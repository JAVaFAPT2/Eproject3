import React, { useEffect, useState, useCallback } from 'react';
import {
  Box,
  Heading,
  Text,
  Grid,
  Icon,
  Container,
  useColorModeValue,
  Skeleton,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import { ArrowForwardIcon } from '@chakra-ui/icons';
import { useNavigate } from 'react-router-dom';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';

const MotionBox = motion(Box);

export default function StartYourJourney() {
  const navigate = useNavigate();
  const [models, setModels] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const bg = useColorModeValue('white', 'navy.900');

  // ✅ Optimized model fetching with deferred loading
  const fetchModels = useCallback(async () => {
    try {
      const data = await VehicleModelService.get();
      const allModels = data?.items || [];
      const topLevelModels = allModels.filter((m) => m.level === 1);

      const enriched = await Promise.all(
        topLevelModels.map(async (m) => {
          try {
            const photos = await VehiclePhotoService.getByModelNumber(
              m.modelNumber,
            );
            const displayPhoto =
              photos.items?.find((p) => p.displayOrder === 1)?.photoUrl ||
              photos.items?.[0]?.photoUrl ||
              photos.items?.[0]?.url ||
              '/placeholder-car.png';

            return { ...m, photo: displayPhoto, photos };
          } catch {
            return { ...m, photo: '/placeholder-car.png', photos: [] };
          }
        }),
      );

      setModels(enriched);
    } catch (err) {
      console.error('Error fetching vehicle models:', err);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    // Defer API calls to improve initial LCP
    const timer = setTimeout(fetchModels, 1500);
    return () => clearTimeout(timer);
  }, [fetchModels]);

  return (
    <Box bg={bg} maxW="1880px" mx={{ base: 5, md: 20 }} position="relative">
      <Container maxW="6xl" textAlign="center" mb={{ base: 10, md: 20 }}>
        <Heading
          as="h2"
          fontSize={{ base: '3xl', md: '6xl' }}
          fontWeight="400"
          lineHeight="shorter"
          mb={4}
        >
          Your journey starts now.
        </Heading>
      </Container>

      {/* ✅ Hiển thị danh sách model */}
      <Box px={{ base: 0, md: 10 }} pb={16}>
        <Grid
          templateColumns={{ base: '1fr', lg: 'repeat(2, 1fr)' }}
          gap={10}
          justifyItems="center"
        >
          {isLoading ? (
            // Loading skeletons
            Array.from({ length: 2 }).map((_, idx) => (
              <Skeleton key={idx} height="60vh" borderRadius="2xl" />
            ))
          ) : (
            models.map((m, idx) => (
            <MotionBox
              key={m.modelNumber}
              w="100%"
              borderRadius="2xl"
              overflow="hidden"
              cursor="pointer"
              position="relative"
              onClick={() => navigate(`/user/models?parentModelNumber=${m.modelNumber}`)}
              initial={{ opacity: 0, y: 40 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{
                duration: 0.6,
                delay: idx * 0.1,
                ease: 'easeOut',
              }}
            >
              <Box position="relative" h={{ base: '90vh', md: '60vh' }}>
                <motion.img
                  src={m.photo}
                  alt={m.name}
                  style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                  initial={{ scale: 1.05 }}
                  whileHover={{ scale: 1.1 }}
                  transition={{ duration: 0.6 }}
                  loading="lazy"
                  decoding="async"
                />

                {/* Overlay tên model */}
                <Grid
                  position="absolute"
                  top={0}
                  left={0}
                  w="100%"
                  h="25%"
                  bgGradient="linear(to-b, rgba(0,0,0,0.7), transparent)"
                  placeItems="center"
                  pt={4}
                >
                  <Text
                    fontSize={{ base: '2xl', md: '4xl' }}
                    color="white"
                    fontFamily="'Kaushan Script', cursive"
                    fontWeight="600"
                  >
                    {m.name}
                  </Text>
                </Grid>

                {/* Overlay dưới */}
                <Grid
                  position="absolute"
                  bottom={0}
                  left={0}
                  w="100%"
                  bgGradient="linear(to-t, rgba(0,0,0,0.85), transparent)"
                  color="white"
                  p={{ base: 5, md: 6 }}
                  gap={2}
                >
                  <Text fontSize={{ base: 'sm', md: 'md' }}>
                    {m.description}
                  </Text>
                  <Grid templateColumns="auto auto" alignItems="center" gap={2}>
                    <Text fontWeight="600">Explore</Text>
                    <Icon as={ArrowForwardIcon} boxSize={5} />
                  </Grid>
                </Grid>
              </Box>
            </MotionBox>
            ))
          )}
        </Grid>
      </Box>
    </Box>
  );
}
