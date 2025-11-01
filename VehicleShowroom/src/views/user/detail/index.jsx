import React, { useEffect, useState, useMemo } from 'react';
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
  Skeleton,
} from '@chakra-ui/react';
import { useParams, useNavigate } from 'react-router-dom';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';
import VehicleSpecService from 'services/VehicleSpecService';
import VSpec from 'views/user/detail/components/VSpec';
import TechnicalDrawer from 'views/user/detail/components/TechnicalDrawer';
import PurchaseDrawer from 'views/user/detail/components/PurchaseDrawer';
import InteriorSlider from 'views/user/detail/components/InteriorSlider'; // ✅ import component slider nội thất
import { useAppToast } from 'utils/ToastHelper';
import { useUser } from 'contexts/UserContext';

export default function Detail() {
  const { slug } = useParams();
  const [model, setModel] = useState(null);
  const [photos, setPhotos] = useState([]);
  const [specs, setSpecs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadingPhotos, setLoadingPhotos] = useState(true);
  const [loadingSpecs, setLoadingSpecs] = useState(true);

  const [isTechOpen, setTechOpen] = useState(false);
  const { isOpen, onOpen, onClose } = useDisclosure();
  const navigate = useNavigate();
  const toast = useAppToast();
  const { isAuthenticated, loading: userLoading } = useUser();

  // 📦 Load data (model, photos, specs)
  useEffect(() => {
    let isMounted = true;
    async function loadModelData() {
      try {
        setLoading(true);
        setLoadingPhotos(true);
        setLoadingSpecs(true);
        const data = await VehicleModelService.getBySlug(slug);
        if (!isMounted) return;
        setModel(data);

        const [photoRes, specRes] = await Promise.all([
          VehiclePhotoService.getByModelNumber(data.modelNumber),
          VehicleSpecService.getByModelNumber(data.modelNumber),
        ]);

        if (isMounted) {
          setPhotos(photoRes?.items || photoRes || []);
          setSpecs(specRes?.items || specRes || []);
        }
      } catch (err) {
        console.error(err);
        toast.error('Failed to load vehicle details');
      } finally {
        if (isMounted) {
          setLoading(false);
          setLoadingPhotos(false);
          setLoadingSpecs(false);
        }
      }
    }
    if (slug) loadModelData();
    return () => (isMounted = false);
  }, [slug]);

  // ✅ Filter ảnh thật (không placeholder)
  const isValidPhotoUrl = (url) => {
    if (!url) return false;
    const invalidPatterns = [
      'placeholder',
      'no-image',
      'via.placeholder',
      'dummyimage',
      'text=No+Image',
      'text=No Image',
      'placehold.co',
    ];
    return !invalidPatterns.some((p) => url.toLowerCase().includes(p));
  };

  const realPhotos = useMemo(
    () => photos.filter((p) => isValidPhotoUrl(p.photoUrl)),
    [photos],
  );

  const mainPhoto =
    realPhotos.find((p) => p.displayOrder === 0)?.photoUrl ||
    realPhotos[0]?.photoUrl;
  const sidePhotos = realPhotos.filter((p) => p.displayOrder === 1);

  // ✅ Ảnh nội thất (displayOrder > 1)
  const interiorPhotos = useMemo(() => {
    const filtered = realPhotos.filter(
      (p) =>
        (typeof p.displayOrder === 'number' && p.displayOrder > 1) ||
        p.category?.toLowerCase() === 'interior' ||
        p.photoType?.toLowerCase() === 'interior' ||
        p.photoUrl.toLowerCase().includes('interior'),
    );

    // Nếu không có ảnh tagged interior, fallback lấy sau 3 ảnh đầu
    if (filtered.length === 0 && realPhotos.length > 3) {
      return realPhotos.slice(3);
    }
    return filtered;
  }, [realPhotos]);

  // ✅ Spec groups
  const accelSpec = specs.find((s) => s.specName === 'Acceleration 0 - 100 km/h');
  const powerKW = specs.find((s) => s.specName === 'Power (kW)');
  const powerPS = specs.find((s) => s.specName === 'Power (PS)');
  const topSpeed = specs.find((s) => s.specName === 'Top speed');

  const performanceSpecs = [
    accelSpec && { head: accelSpec.specName, sub: accelSpec.specValue },
    (powerKW || powerPS) && {
      head: 'Power (kW / PS)',
      sub:
        powerKW && powerPS
          ? `${powerKW.specValue} kW / ${powerPS.specValue} PS`
          : powerKW
          ? `${powerKW.specValue} kW`
          : `${powerPS.specValue} PS`,
    },
    topSpeed && { head: topSpeed.specName, sub: topSpeed.specValue },
  ].filter(Boolean);

  const groupedSpecs =
    specs?.reduce((acc, spec) => {
      acc[spec.groupName] = acc[spec.groupName] || [];
      acc[spec.groupName].push(spec);
      return acc;
    }, {}) || {};

  const handleContact = () => {
    if (!isAuthenticated) {
      toast.warning('Please sign in first');
      navigate('/auth/sign-in');
      return;
    }
    onOpen();
  };

  if (loading || userLoading)
    return (
      <Box maxW="7xl" mx="auto" p={6} pt="220px">
        <Skeleton height="400px" borderRadius="xl" mb={10} />
        <Skeleton height="40px" width="300px" />
      </Box>
    );

  if (!model) return <Text textAlign="center">Model not found</Text>;

  return (
    <Box maxW="7xl" mx="auto" p={6} pt="220px">
      {/* 🏎️ Ảnh chính */}
      {mainPhoto ? (
        <Image
          src={mainPhoto}
          alt={model.name}
          w="100%"
          borderRadius="xl"
          mb={10}
        />
      ) : (
        <Box h="400px" bg="gray.100" borderRadius="xl" />
      )}

      {/* 🏷️ Thông tin xe */}
      <VStack align="center" spacing={1} mb={10}>
        <Text fontSize="6xl" fontWeight="700">
          {model.name}
        </Text>
        <Text fontWeight="500" fontSize="lg" mt={2}>
          from ${model.price?.toLocaleString()}
        </Text>
      </VStack>

      {/* ⚙️ Thông số + ảnh phụ */}
      <Flex
        justify="space-between"
        align="flex-start"
        gap={10}
        flexDir={{ base: 'column', lg: 'row' }}
      >
        {/* Cột trái */}
        <Box flex="2">
          <VStack align="start" spacing={3} mb={6}>
            {performanceSpecs.map((spec, idx) => (
              <VSpec key={idx} head={spec.head} sub={spec.sub} />
            ))}
          </VStack>
          <HStack spacing={4} mt={4}>
            <Button
              variant="outline"
              borderColor="black"
              onClick={() => setTechOpen(true)}
            >
              View all technical details
            </Button>
            <Button bg="black" color="white" onClick={handleContact}>
              Contact
            </Button>
          </HStack>
        </Box>

        {/* Cột phải - Exterior */}
        <Box flex="3" display="flex" flexDir="column" gap={6}>
          {sidePhotos.length > 0 ? (
            sidePhotos.map((p, i) => (
              <Image
                key={i}
                src={p.photoUrl}
                alt={`${model.name} exterior ${i}`}
                borderRadius="lg"
                w="100%"
                h="auto"
                objectFit="cover"
              />
            ))
          ) : (
            <Text color="gray.500">No exterior photos available</Text>
          )}
        </Box>
      </Flex>

      {/* 🪟 Slider nội thất (displayOrder > 1) */}
      <InteriorSlider photos={interiorPhotos} />

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
