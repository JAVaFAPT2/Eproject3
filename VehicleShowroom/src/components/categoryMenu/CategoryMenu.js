import React, { useEffect, useState } from 'react';
import {
  Box,
  Flex,
  IconButton,
  Button,
  useColorModeValue,
  HStack,
  Text,
  Grid,
  GridItem,
  Image,
  Heading,
  VStack,
  Tag,
  Spinner,
} from '@chakra-ui/react';
import { CloseIcon, ArrowBackIcon } from '@chakra-ui/icons';
import { motion, AnimatePresence } from 'framer-motion';
import { useNavigate } from 'react-router-dom';
import { MdLogin, MdPerson, MdLogout } from 'react-icons/md';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';
import VehicleSpecService from 'services/VehicleSpecService';
import { useUser } from 'contexts/UserContext';

const MotionBox = motion(Box);

export default function CategoryMenu({ isVisible, closeHandler }) {
  const bgColor = useColorModeValue('white', 'gray.900');
  const textColor = useColorModeValue('gray.700', 'white');
  const borderColor = useColorModeValue('gray.200', 'gray.700');
  const navigate = useNavigate();
  const { isAuthenticated, user, logout } = useUser();

  const [loading, setLoading] = useState(true);
  const [allModels, setAllModels] = useState([]); // cấp 1
  const [displayedModels, setDisplayedModels] = useState([]);
  const [parentModel, setParentModel] = useState(null);

  // 🟢 Fetch cấp 1
  useEffect(() => {
    const fetchLevel1Models = async () => {
      setLoading(true);
      try {
        const res = await VehicleModelService.get({ pageNumber: 1, pageSize: 100 });
        const models = res?.items?.filter((m) => m.level === 1) || [];

        const enriched = await Promise.all(
          models.map(async (m) => {
            let photoUrl = m.photo || 'https://placehold.co/600x400?text=No+Image';
            try {
              const photos = await VehiclePhotoService.getByModelNumber(m.modelNumber);
              const displayPhoto =
                photos.items?.find((p) => p.displayOrder === 0)?.photoUrl ||
                photos.items?.[0]?.photoUrl ||
                photos.items?.[0]?.url;
              if (displayPhoto) photoUrl = displayPhoto;
            } catch {}
            return { ...m, photo: photoUrl };
          })
        );

        setAllModels(enriched);
        setDisplayedModels(enriched);
      } catch (err) {
        console.error('❌ Error fetching level 1 models:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchLevel1Models();
  }, []);

  // 🟣 Fetch cấp 2 (variants)
  const handleOpenLevel2 = async (parentModelNumber, name) => {
    console.log('➡ Fetching level 2 for', parentModelNumber);
    setLoading(true);
    try {
      const res = await VehicleModelService.get({ parentModelNumber });
      const variants = res?.items || res; // fallback nếu API trả mảng thẳng

      if (!variants.length) {
        console.warn('⚠️ No variants found for', parentModelNumber);
      }

      const enrichedVariants = await Promise.all(
        variants.map(async (m) => {
          let photoUrl = m.photo || 'https://placehold.co/600x400?text=No+Image';
          let fuelType = 'N/A';

          try {
            const photos = await VehiclePhotoService.getByModelNumber(m.modelNumber);
            const displayPhoto =
              photos.items?.find((p) => p.displayOrder === 0)?.photoUrl ||
              photos.items?.[0]?.photoUrl ||
              photos.items?.[0]?.url;
            if (displayPhoto) photoUrl = displayPhoto;
          } catch {}

          try {
            const specs = await VehicleSpecService.getByModelNumber(m.modelNumber);
            const fuelSpec = specs.items.find((s) => s.specName === 'Fuel Type');
            if (fuelSpec) fuelType = fuelSpec.specValue;
          } catch {}

          return { ...m, photo: photoUrl, fuelType };
        })
      );

      setDisplayedModels(enrichedVariants);
      setParentModel({ modelNumber: parentModelNumber, name });
    } catch (err) {
      console.error('❌ Error fetching submodels:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleBack = () => {
    setDisplayedModels(allModels);
    setParentModel(null);
  };

  const handleSignOut = async () => {
    await logout();
    closeHandler();
  };

  const handleNavigate = (path) => {
    closeHandler();
    navigate(path);
  };

  // 🌀 Render nội dung
  const renderContent = () => {
    if (loading)
      return (
        <Flex justify="center" align="center" h="100%">
          <Spinner size="lg" />
        </Flex>
      );

    if (!displayedModels.length)
      return (
        <Text color="gray.500" fontStyle="italic" textAlign="center" py={4}>
          No models found
        </Text>
      );

    return (
      <Grid templateColumns="1fr" gap={6} placeItems="center" pb={8}>
        {displayedModels.map((el) => (
          <GridItem
            key={el.modelNumber}
            w="full"
            maxW="28rem"
            borderRadius="md"
            transition="0.25s ease"
            cursor="pointer"
            p={4}
            role="group"
            _hover={{ bg: '#eeeff2' }}
          >
            <VStack align="start" spacing={3}>
              {/* 🔹 Tên model */}
              <Heading
                size="md"
                fontWeight="semibold"
                role="button"
                onClick={() => {
                  if (el.level === 1) {
                    handleOpenLevel2(el.modelNumber, el.name);
                  } else if (el.level === 2 && el.slug) {
                    closeHandler();
                    navigate(`/user/detail/${el.slug}`);
                  }
                }}
                _hover={{ color: 'brand.500' }}
              >
                {el.name}
              </Heading>

              {/* 🔹 Ảnh */}
              <Box
                w="full"
                overflow="hidden"
                borderRadius="md"
                role="button"
                onClick={() => {
                  if (el.level === 1) {
                    handleOpenLevel2(el.modelNumber, el.name);
                  } else if (el.level === 2 && el.slug) {
                    closeHandler();
                    navigate(`/user/detail/${el.slug}`);
                  }
                }}
              >
                <Image
                  src={el.photo || 'https://placehold.co/600x400?text=No+Image'}
                  alt={el.name}
                  w="full"
                  h="auto"
                  objectFit="cover"
                  borderRadius="md"
                  transition="transform 0.3s ease"
                  _groupHover={{ transform: 'translateX(10px)' }}
                />
              </Box>

              {/* 🔹 Fuel type */}
              {el.level === 2 && (
                <Flex gap={2} wrap="wrap">
                  <Tag bg="gray.200" color="black" fontWeight="medium">
                    {el.fuelType || 'N/A'}
                  </Tag>
                </Flex>
              )}
            </VStack>
          </GridItem>
        ))}
      </Grid>
    );
  };

  return (
    <>
      <AnimatePresence>
        {isVisible && (
          <MotionBox
            initial={{ x: '-100%', opacity: 0 }}
            animate={{ x: 0, opacity: 1 }}
            exit={{ x: '-100%', opacity: 0 }}
            transition={{ duration: 0.35, ease: 'easeInOut' }}
            position="fixed"
            top="0"
            left="0"
            h="100dvh"
            w={{ base: '100%', md: '30%' }}
            bg={bgColor}
            color={textColor}
            shadow="xl"
            display="flex"
            flexDirection="column"
            justifyContent="space-between"
            zIndex="1500"
          >
            {/* 🔹 Header */}
            <Flex justify="space-between" align="center" p={6} borderColor={borderColor}>
              {parentModel ? (
                <Flex align="center" gap={3}>
                  <IconButton
                    icon={<ArrowBackIcon />}
                    aria-label="Back"
                    onClick={handleBack}
                    variant="ghost"
                    size="sm"
                  />
                  <Text fontSize="xl" fontWeight="600">
                    {parentModel.name}
                  </Text>
                </Flex>
              ) : (
                <Text fontSize="2xl" fontWeight="600">
                  Models
                </Text>
              )}

              <IconButton
                icon={<CloseIcon />}
                aria-label="Close menu"
                onClick={closeHandler}
                variant="ghost"
                size="sm"
              />
            </Flex>

            {/* 🔹 Content */}
            <Box flex="1" overflowY="auto">
              {renderContent()}
            </Box>

            {/* 🔹 Footer */}
            <Flex
              px="1rem"
              py="1rem"
              borderTop="1px solid"
              borderColor={borderColor}
              justify="space-between"
              align="center"
              gap={3}
            >
              {!isAuthenticated ? (
                <Flex w="full" justify="flex-end">
                  <Button
                    rightIcon={<MdLogin size={20} />}
                    onClick={() => handleNavigate('/auth/sign-in')}
                  >
                    Sign In
                  </Button>
                </Flex>
              ) : (
                <HStack w="full" justify="space-between">
                  <Flex gap={3} align="center">
                    <Button
                      leftIcon={<MdPerson size={20} />}
                      onClick={() => handleNavigate('/user/profile')}
                    >
                      Profile
                    </Button>
                    {user?.role === 'Admin' && (
                      <Button
                        leftIcon={<MdPerson size={20} />}
                        colorScheme="brand"
                        onClick={() => handleNavigate('/admin')}
                      >
                        Admin Panel
                      </Button>
                    )}
                  </Flex>
                  <Button
                    leftIcon={<MdLogout size={20} />}
                    onClick={handleSignOut}
                    color="red"
                  >
                    Sign Out
                  </Button>
                </HStack>
              )}
            </Flex>
          </MotionBox>
        )}
      </AnimatePresence>

      {/* Overlay */}
      <AnimatePresence>
        {isVisible && (
          <MotionBox
            initial={{ opacity: 0 }}
            animate={{ opacity: 0.3 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
            position="fixed"
            top="0"
            left={{ base: '0', md: '30%' }}
            w={{ base: '100%', md: '70%' }}
            h="100dvh"
            bg="black"
            zIndex="1400"
            onClick={closeHandler}
          />
        )}
      </AnimatePresence>
    </>
  );
}
