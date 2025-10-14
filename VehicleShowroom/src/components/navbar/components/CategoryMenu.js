import React, { useEffect, useState } from 'react';
import {
  Box,
  Flex,
  Grid,
  Text,
  IconButton,
  Button,
  Spinner,
  useColorModeValue,
  Image,
} from '@chakra-ui/react';
import { motion, AnimatePresence } from 'framer-motion';
import { CloseIcon, ArrowBackIcon } from '@chakra-ui/icons';
import { useNavigate } from 'react-router-dom';
import VehicleModelService from 'services/VehicleModelService';
import VehicleService from 'services/VehicleService';
import { MdLogin } from 'react-icons/md';

const MotionBox = motion(Box);
const MotionImage = motion(Image);

export default function CategoryMenu({ isVisible, closeHandler }) {
  const [models, setModels] = useState([]);
  const [vehicles, setVehicles] = useState([]);
  const [selectedModel, setSelectedModel] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const bg = useColorModeValue('whiteAlpha.900', 'gray.800');
  const navigate = useNavigate();

  // ✅ Fetch vehicle models
  useEffect(() => {
    async function fetchModels() {
      try {
        setLoading(true);
        const data = await VehicleModelService.getAll();
        setModels(data || []);
      } catch (err) {
        console.error('Error fetching vehicle models:', err);
        setError('Failed to load models');
      } finally {
        setLoading(false);
      }
    }

    if (isVisible) fetchModels();
  }, [isVisible]);

  // ✅ Khi chọn 1 model → load vehicles thuộc model đó
  const handleModelClick = async (model) => {
    try {
      setLoading(true);
      setSelectedModel(model);

      const all = await VehicleService.getAll();
      const filtered = all.filter((v) =>
        v.modelNumber.toLowerCase().includes(model.modelNumber.toLowerCase()),
      );

      setVehicles(filtered);
    } catch (err) {
      console.error(err);
      setError('Failed to load vehicles');
    } finally {
      setLoading(false);
    }
  };

  const handleVehicleClick = (vehicleId) => {
    closeHandler();
    navigate(`/user/detail/${vehicleId}`);
  };

  const handleBack = () => {
    setSelectedModel(null);
    setVehicles([]);
  };

  const handleSignInClick = () => {
    closeHandler();
    navigate('/auth/sign-in');
  };

  return (
    <AnimatePresence>
      {isVisible && (
        <>
          {/* Overlay */}
          <MotionBox
            position="fixed"
            inset="0"
            bg="blackAlpha.500"
            backdropFilter="blur(8px)"
            zIndex={900}
            onClick={closeHandler}
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.3 }}
          />

          {/* Drawer */}
          <MotionBox
            position="fixed"
            top="0"
            left="0"
            h="100vh"
            w={{ base: '100%', md: '35%' }}
            bg={bg}
            backdropFilter="blur(24px)"
            zIndex={1000}
            borderRightWidth="1px"
            display="flex"
            flexDirection="column"
            justifyContent="space-between"
            initial={{ x: '-100%', opacity: 0 }}
            animate={{ x: 0, opacity: 1 }}
            exit={{ x: '-100%', opacity: 0 }}
            transition={{ duration: 0.45, ease: [0.075, 0.82, 0.165, 1] }}
          >
            {/* Scrollable content */}
            <Box
              flex="1"
              overflowY="auto"
              px={{ base: 6, md: 10 }}
              py={{ base: 4, md: 8 }}
            >
              <Flex justify="space-between" mb={6} align="center">
                <Flex align="center" gap={2}>
                  {selectedModel && (
                    <IconButton
                      aria-label="Back"
                      icon={<ArrowBackIcon boxSize={6} />} 
                      onClick={handleBack}
                      variant="ghost"
                      size="lg" 
                      _hover={{ bg: 'blackAlpha.100' }}
                    />
                  )}
                  <Text
                    fontSize={{ base: '2xl', md: '3xl' }}
                    fontWeight="semibold"
                  >
                    {selectedModel ? selectedModel.name : 'Explore Models'}
                  </Text>
                </Flex>

                <IconButton
                  aria-label="Close menu"
                  variant="ghost"
                  icon={<CloseIcon boxSize={5} />}
                  onClick={closeHandler}
                  _hover={{ bg: 'blackAlpha.100' }}
                />
              </Flex>

              {/* Loading & error */}
              {loading && (
                <Flex justify="center" align="center" py={20}>
                  <Spinner size="xl" />
                </Flex>
              )}

              {error && (
                <Flex justify="center" align="center" py={20}>
                  <Text color="red.500">{error}</Text>
                </Flex>
              )}

              {/* ✅ Hiển thị danh sách model hoặc vehicle */}
              {!loading && !error && (
                <>
                  {/* 🔹 Nếu chưa chọn model → hiển thị danh sách model */}
                  {!selectedModel ? (
                    <Grid templateColumns="1fr" gap={6} pb={10}>
                      {models.map((m) => (
                        <MotionBox
                          key={m.modelNumber}
                          borderRadius="xl"
                          overflow="hidden"
                          p={4}
                          cursor="pointer"
                          transition="all 0.25s ease"
                          _hover={{ backgroundColor: 'white' }}
                          onClick={() => handleModelClick(m)}
                        >
                          <Text fontSize="2xl" fontWeight="semibold" mb={3}>
                            {m.name}
                          </Text>

                          <Box
                            position="relative"
                            overflow="hidden"
                            borderRadius="md"
                            mb={2}
                          >
                            <MotionImage
                              src={m.photos?.[0]?.url || '/placeholder-car.png'}
                              alt={m.name}
                              objectFit="contain"
                              w="100%"
                              h="180px"
                              transition={{ duration: 0.2, ease: 'easeOut' }}
                              whileHover={{ x: 8 }}
                            />
                          </Box>
                        </MotionBox>
                      ))}
                    </Grid>
                  ) : (
                    /* 🔹 Nếu đã chọn model → hiển thị danh sách xe */
                    <Grid templateColumns="1fr" gap={6} pb={10}>
                      {vehicles.map((v) => {
                        const fuelType =
                          v.specs?.find((s) => s.specName === 'Fuel Type')
                            ?.specValue || 'Unknown';
                        const mainPhoto =
                          v.photos?.[0]?.url || '/placeholder-car.png';

                        return (
                          <MotionBox
                            key={v.vehicleId}
                            borderRadius="xl"
                            overflow="hidden"
                            p={4}
                            cursor="pointer"
                            transition="all 0.25s ease"
                            _hover={{ backgroundColor: 'white' }}
                            onClick={() => handleVehicleClick(v.vehicleId)}
                          >
                            {/* 🔹 Tên xe + tag fuel */}
                            <Flex justify="space-between" align="center" mb={3}>
                              <Text fontSize="xl" fontWeight="semibold">
                                {v.name}
                              </Text>
                            </Flex>

                            {/* 🔹 Ảnh xe */}
                            <Box
                              position="relative"
                              overflow="hidden"
                              borderRadius="md"
                              mb={2}
                            >
                              <MotionImage
                                src={mainPhoto}
                                alt={v.name}
                                objectFit="contain"
                                w="100%"
                                h="180px"
                                transition={{
                                  duration: 0.2,
                                  ease: 'easeOut',
                                }}
                                whileHover={{ x: 8 }}
                              />
                            </Box>

                            <Box
                              bg="gray.200"
                              px={2}
                              py={0.5}
                              borderRadius="md"
                              fontSize="xs"
                              fontWeight="500"
                              w="fit-content"
                            >
                              {fuelType}
                            </Box>
                          </MotionBox>
                        );
                      })}

                      {vehicles.length === 0 && (
                        <Text color="gray.400" fontStyle="italic">
                          No vehicles found for this model.
                        </Text>
                      )}
                    </Grid>
                  )}
                </>
              )}
            </Box>

            {/* Footer Sign-in */}
            <Box
              px={{ base: 6, md: 10 }}
              py={4}
              bg="transparent"
              position="sticky"
              bottom="0"
            >
              <Flex
                align="center"
                justify="space-between"
                onClick={handleSignInClick}
                _hover={{ cursor: 'pointer' }}
              >
                <Button
                  variant="ghost"
                  colorScheme="transparent"
                  fontWeight="600"
                  fontSize="lg"
                >
                  Sign In
                </Button>
                <MdLogin size={30} />
              </Flex>
            </Box>
          </MotionBox>
        </>
      )}
    </AnimatePresence>
  );
}
