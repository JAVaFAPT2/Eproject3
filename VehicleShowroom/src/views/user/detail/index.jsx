import React, { useEffect, useState } from 'react';
import {
  Box,
  Image,
  Text,
  VStack,
  Button,
  Flex,
  HStack,
  useDisclosure,
  Spinner,
} from '@chakra-ui/react';
import { useParams, useNavigate } from 'react-router-dom';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';
import VehicleSpecService from 'services/VehicleSpecService';
import VSpec from 'views/user/detail/components/VSpec';
import TechnicalDrawer from 'views/user/detail/components/TechnicalDrawer';
import PurchaseDrawer from 'views/user/detail/components/PurchaseDrawer';
import { useAppToast } from 'utils/ToastHelper';
import { useUser } from 'contexts/UserContext'; // ✅ import từ context

export default function Detail() {
  const { slug } = useParams();
  const [model, setModel] = useState(null);
  const [photos, setPhotos] = useState([]);
  const [specs, setSpecs] = useState([]);
  const [loading, setLoading] = useState(true);

  const [isTechOpen, setTechOpen] = useState(false);
  const { isOpen, onOpen, onClose } = useDisclosure();
  const navigate = useNavigate();
  const toast = useAppToast();

  // ✅ Lấy user context
  const { isAuthenticated, loading: userLoading } = useUser();

  useEffect(() => {
    async function loadModelData() {
      setLoading(true);
      try {
        const data = await VehicleModelService.getBySlug(slug);
        setModel(data);

        const photosRes = await VehiclePhotoService.getByModelNumber(
          data.modelNumber,
        );
        setPhotos(photosRes.items);

        const specsRes = await VehicleSpecService.getByModelNumber(
          data.modelNumber,
        );
        setSpecs(specsRes.items);
      } catch (err) {
        console.error('Failed to load vehicle model detail:', err);
        toast.error('Failed to load vehicle model detail');
      } finally {
        setLoading(false);
      }
    }

    if (slug) loadModelData();
  }, [slug]);

  // ✅ Loading state
  if (loading || userLoading)
    return (
      <Flex justify="center" align="center" h="80vh">
        <Spinner size="xl" />
      </Flex>
    );

  if (!model)
    return (
      <Text textAlign="center" mt={10}>
        Model not found.
      </Text>
    );

  // 🔹 Ảnh chính và ảnh phụ
  const mainPhoto =
    photos.find((p) => p.displayOrder === 0)?.photoUrl ||
    photos[0]?.photoUrl ||
    '/placeholder-car.png';
  const sidePhotos = photos.slice(1) || [];

  // 🔹 Lấy 3 thông số chính
  const accelSpec = specs.find(
    (s) => s.specName === 'Acceleration 0 - 100 km/h',
  );
  const powerKW = specs.find((s) => s.specName === 'Power (kW)');
  const powerPS = specs.find((s) => s.specName === 'Power (PS)');
  const topSpeed = specs.find((s) => s.specName === 'Top speed');

  const performanceSpecs = [
    accelSpec && {
      head: 'Acceleration 0 - 100 km/h',
      sub: accelSpec.specValue,
    },
    (powerKW || powerPS) && {
      head: 'Power (kW) / Power (PS)',
      sub:
        powerKW && powerPS
          ? `${powerKW.specValue} kW / ${powerPS.specValue} PS`
          : powerKW
          ? `${powerKW.specValue} kW`
          : powerPS
          ? `${powerPS.specValue} PS`
          : 'N/A',
    },
    topSpeed && { head: 'Top speed', sub: topSpeed.specValue },
  ].filter(Boolean);

  // 🔹 Group specs cho TechnicalDrawer
  const groupedSpecs =
    specs.reduce((acc, spec) => {
      acc[spec.groupName] = acc[spec.groupName] || [];
      acc[spec.groupName].push(spec);
      return acc;
    }, {}) || {};

  // ✅ Check đăng nhập qua context
  const handleContact = () => {
    if (!isAuthenticated) {
      toast.warning('Please sign in first');
      navigate('/auth/sign-in');
      return;
    }
    onOpen();
  };

  return (
    <Box maxW="7xl" mx="auto" p={6} pt="220px">
      {/* Ảnh chính */}
      <Image
        src={mainPhoto}
        alt={model.name}
        w="100%"
        h="auto"
        borderRadius="xl"
        mb={10}
      />

      {/* Thông tin xe */}
      <VStack align="center" spacing={1} mb={10}>
        <Text fontSize="6xl" fontWeight="700">
          {model.name}
        </Text>

        {specs && (
          <Box mt={1}>
            <Box
              bg="gray.200"
              px={3}
              py={1}
              borderRadius="md"
              display="inline-block"
            >
              <Text color="black" fontSize="sm" fontWeight="500">
                {specs.find((s) => s.specName === 'Fuel Type')?.specValue ||
                  'N/A'}
              </Text>
            </Box>
          </Box>
        )}

        <Box>
          <Text fontWeight="500" fontSize="lg" mt={4}>
            from ${model.price?.toLocaleString()}
          </Text>
        </Box>
      </VStack>

      {/* Layout specs + ảnh phụ */}
      <Flex
        justify="space-between"
        align="center"
        flexDir={{ base: 'column', md: 'row' }}
      >
        {/* Performance Specs */}
        <Box flex="2">
          <VStack align="start" spacing={3} mb={6}>
            {performanceSpecs.map((spec, idx) => (
              <VSpec key={idx} head={spec.head} sub={spec.sub} />
            ))}
          </VStack>

          {/* Buttons */}
          <HStack spacing={4} mt={4}>
            <Button
              variant="outline"
              colorScheme="blackAlpha"
              borderColor="black"
              borderWidth="2px"
              borderRadius="sm"
              color="black"
              size="lg"
              _hover={{ bg: 'blackAlpha.100' }}
              onClick={() => setTechOpen(true)}
              fontWeight="400"
            >
              View all technical details
            </Button>

            <Button
              bg="black"
              color="white"
              size="lg"
              px={8}
              _hover={{ bg: 'gray.700' }}
              fontWeight="500"
              onClick={handleContact}
            >
              Contact
            </Button>
          </HStack>
        </Box>

        {/* Ảnh phụ */}
        {sidePhotos.length > 0 && (
          <Box flex="3" display="flex" flexDir="column" gap={6}>
            {sidePhotos.map((photo, idx) => (
              <Image
                key={idx}
                src={photo.photoUrl}
                alt={`${model.name} view ${idx + 1}`}
                w="100%"
                borderRadius="lg"
              />
            ))}
          </Box>
        )}
      </Flex>

      {/* Drawer kỹ thuật */}
      <TechnicalDrawer
        isOpen={isTechOpen}
        onClose={() => setTechOpen(false)}
        groupedSpecs={groupedSpecs}
      />

      {/* Drawer form mua xe */}
      <PurchaseDrawer isOpen={isOpen} onClose={onClose} vehicle={model} />
    </Box>
  );
}
