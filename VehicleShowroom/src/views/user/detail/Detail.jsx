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
  useToast,
} from '@chakra-ui/react';
import { useParams, useNavigate } from 'react-router-dom';
import VehicleService from 'services/VehicleService';
import VSpec from 'views/user/detail/components/VSpec';
import TechnicalDrawer from 'views/user/detail/components/TechnicalDrawer';
import PurchaseDrawer from 'views/user/detail/components/PurchaseDrawer';

export default function Detail() {
  const { id } = useParams();
  const [vehicle, setVehicle] = useState(null);
  const [isTechOpen, setTechOpen] = useState(false);
  const { isOpen, onOpen, onClose } = useDisclosure(); // cho form contact
  const navigate = useNavigate();
  const toast = useToast();

  useEffect(() => {
    async function loadVehicle() {
      const all = await VehicleService.getAll();
      const v = all.find((x) => x.vehicleId === id);
      setVehicle(v);
    }
    loadVehicle();
  }, [id]);

  if (!vehicle) return <Text p={10}>Loading...</Text>;

  const mainPhoto = vehicle.photos?.[0]?.url || '/placeholder-car.png';
  const sidePhotos = vehicle.photos?.slice(1) || [];

  const performanceSpecs =
    vehicle.specs?.filter((s) => s.groupName === 'GeneralPerformance') || [];

  const groupedSpecs =
    vehicle.specs?.reduce((acc, spec) => {
      acc[spec.groupName] = acc[spec.groupName] || [];
      acc[spec.groupName].push(spec);
      return acc;
    }, {}) || {};

  // ✅ Giả lập kiểm tra đăng nhập
  const isAuthenticated = !!localStorage.getItem('access_token');

  const handleContact = () => {
    if (!isAuthenticated) {
      toast({
        title: 'Please sign in first',
        description:
          'You must be logged in to purchase or contact about a vehicle.',
        status: 'warning',
        duration: 3000,
        isClosable: true,
      });
      navigate('/signin');
      return;
    }
    onOpen(); // mở form
  };

  return (
    <Box maxW="7xl" mx="auto" p={6} pt="220px">
      {/* Ảnh chính */}
      <Image
        src={mainPhoto}
        alt={vehicle.name}
        w="100%"
        h="auto"
        borderRadius="xl"
        mb={10}
      />

      {/* Thông tin xe */}
      <VStack align="center" spacing={1} mb={10}>
        <Text fontSize="6xl" fontWeight="700">
          {vehicle.name}
        </Text>

        {vehicle.specs && (
          <Box mt={1}>
            <Box
              bg="gray.200"
              px={3}
              py={1}
              borderRadius="md"
              display="inline-block"
            >
              <Text color="black" fontSize="sm" fontWeight="500">
                {
                  vehicle.specs.find((s) => s.specName === 'Fuel Type')
                    ?.specValue
                }
              </Text>
            </Box>
          </Box>
        )}

        <Box>
          <Text fontWeight="500" fontSize="lg" mt={4}>
            from ${vehicle.purchasePrice?.toLocaleString()}
          </Text>
        </Box>
      </VStack>

      {/* Layout specs + ảnh phụ */}
      <Flex gap={10} align="start" flexDir={{ base: 'column', md: 'row' }}>
        {/* Performance Specs */}
        <Box flex="1">
          <Text fontSize="2xl" fontWeight="600" mb={4}>
            Key Performance Specs
          </Text>
          <VStack align="start" spacing={3} mb={6}>
            {performanceSpecs.map((spec) => (
              <VSpec
                key={spec.specId}
                head={spec.specName}
                sub={spec.specValue}
              />
            ))}
          </VStack>

          {/* 🔹 Buttons */}
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
          <Box flex="2" display="flex" flexDir="column" gap={6}>
            {sidePhotos.map((photo, idx) => (
              <Image
                key={idx}
                src={photo.url}
                alt={`${vehicle.name} view ${idx + 1}`}
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
      <PurchaseDrawer isOpen={isOpen} onClose={onClose} vehicle={vehicle} />
    </Box>
  );
}
