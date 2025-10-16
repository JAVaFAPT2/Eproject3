import React, { useEffect, useState } from 'react';
import {
  Box,
  Heading,
  Text,
  Grid,
  Icon,
  Container,
  useColorModeValue,
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
  const bg = useColorModeValue('white', 'navy.900');

  // ✅ Fetch models and their photos
  useEffect(() => {
    async function fetchModels() {
      try {
        const data = await VehicleModelService.get();
        const list = data.vehicleModels || [];

        // Fetch photos for each model in parallel
        const enriched = await Promise.all(
          list.map(async (m) => {
            try {
              const photos = await VehiclePhotoService.getByModelNumber(
                m.modelNumber,
              );
              return { ...m, photos };
            } catch (err) {
              console.warn(`No photos for ${m.modelNumber}`);
              return { ...m, photos: [] };
            }
          }),
        );

        setModels(enriched);
      } catch (err) {
        console.error('Error fetching vehicle models:', err);
      }
    }

    fetchModels();
  }, []);

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

      {/* Vehicle Models */}
      <Box px={{ base: 0, md: 10 }} pb={16}>
        <Grid
          templateColumns={{
            base: '1fr',
            lg: 'repeat(2, 1fr)',
          }}
          gap={10}
          justifyItems="center"
        >
          {models.map((m, idx) => (
            <MotionBox
              key={m.modelNumber}
              w="100%"
              borderRadius="2xl"
              overflow="hidden"
              cursor="pointer"
              position="relative"
              onClick={() => navigate(`/user/models/${m.modelNumber}`)}
              initial={{ opacity: 0, y: 40 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{
                duration: 0.6,
                delay: idx * 0.1,
                ease: 'easeOut',
              }}
            >
              {/* ✅ Background image */}
              <Box
                position="relative"
                h={{ base: '90vh', md: '60vh' }}
                borderRadius="2xl"
                overflow="hidden"
              >
                <motion.img
                  src={
                    m.photos?.[0]?.url ||
                    m.photos?.[0]?.photoUrl ||
                    '/placeholder-car.png'
                  }
                  alt={m.name}
                  style={{
                    width: '100%',
                    height: '100%',
                    objectFit: 'cover',
                  }}
                  initial={{ scale: 1.05 }}
                  whileHover={{ scale: 1.1 }}
                  transition={{ duration: 0.6 }}
                />

                {/* 🔹 Top Overlay (Model Name) */}
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
                    textAlign="center"
                  >
                    {m.name}
                  </Text>
                </Grid>

                {/* 🔹 Bottom Overlay (Description + Explore) */}
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
                  <Grid
                    templateColumns="auto auto"
                    alignItems="center"
                    justifyContent="start"
                    gap={2}
                  >
                    <Text fontWeight="600">Explore</Text>
                    <Icon as={ArrowForwardIcon} boxSize={5} />
                  </Grid>
                </Grid>
              </Box>
            </MotionBox>
          ))}
        </Grid>
      </Box>
    </Box>
  );
}
