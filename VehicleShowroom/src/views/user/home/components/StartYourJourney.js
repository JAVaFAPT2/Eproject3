import React, { useEffect, useState } from 'react';
import {
  Box,
  Heading,
  Text,
  Flex,
  Button,
  Icon,
  Container,
  useColorModeValue,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import { ArrowForwardIcon } from '@chakra-ui/icons';
import { useNavigate } from 'react-router-dom';
import VehicleModelService from 'services/VehicleModelService';

const MotionBox = motion(Box);

export default function StartYourJourney() {
  const navigate = useNavigate();
  const [models, setModels] = useState([]);
  const [paused, setPaused] = useState(false);
  const bg = useColorModeValue('white', 'navy.900');

  // ✅ Fetch data từ VehicleModelService
  useEffect(() => {
    async function fetchData() {
      try {
        const data = await VehicleModelService.getAll();
        setModels(data || []);
      } catch (err) {
        console.error('Error fetching vehicle models:', err);
      }
    }
    fetchData();
  }, []);

  return (
    <Box bg={bg} maxW="1880px" mx={{ base: 10, md: 20 }} position="relative">
      {/* Header */}
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
      <Flex
        overflowX="auto"
        gap={6}
        px={{ base: 4, md: 10 }}
        pb={8}
        justify="center"
        flexWrap={{ base: 'nowrap', md: 'wrap' }}
      >
        {models.map((m, idx) => (
          <MotionBox
            key={m.modelNumber}
            flex={{ base: '0 0 85%', md: '1 1 45%' }}
            borderRadius="xl"
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
            {/* Video / Image Poster */}
            <Box
              position="relative"
              h={{ base: '75vh', md: '45vh' }}
              overflow="hidden"
              borderRadius="xl"
              bg="black"
            >
              <motion.img
                src={m.photos?.[0]?.url || '/placeholder-car.png'}
                alt={m.name}
                style={{
                  width: '100%',
                  height: '100%',
                  objectFit: 'contain',
                }}
                initial={{ scale: 1.05 }}
                whileHover={{ scale: 1.1 }}
                transition={{ duration: 0.6 }}
              />

              {/* 🔹 Model Name (top center) */}
              <Flex
                position="absolute"
                top={4}
                left="50%"
                transform="translateX(-50%)"
                justify="center"
                align="center"
                px={4}
                py={1}
                borderRadius="full"
              >
                <Text
                  fontSize={{ base: 'xl', md: '4xl' }}
                  color="white"
                  textAlign="center"
                  fontFamily="'Kaushan Script', cursive"
                  fontStyle="italic"
                  fontWeight="600"
                >
                  {m.name}
                </Text>
              </Flex>

              {/* 🔹 Description & Explore (bottom row) */}
              <Flex
                position="absolute"
                bottom={0}
                left={0}
                w="100%"
                color="white"
                px={6}
                py={4}
                align="center"
                justify="space-between"
              >
                <Text fontSize={{ base: 'sm', md: 'md' }} mb={2}>
                  {m.description}
                </Text>

                <Flex align="center" gap={2}>
                  <Text fontWeight="500">Explore</Text>
                  <Icon as={ArrowForwardIcon} boxSize={5} />
                </Flex>
              </Flex>
            </Box>
          </MotionBox>
        ))}
      </Flex>

      {/* Bottom Controller (mobile only) */}
      <Flex
        display={{ base: 'flex', md: 'none' }}
        justify="center"
        align="center"
        mt={4}
        gap={3}
        flexDir="column"
      >
        {/* Dots */}
        <Flex
          bg="rgba(148,149,153,0.18)"
          borderRadius="full"
          px={4}
          py={2}
          gap={3}
          align="center"
          justify="center"
        >
          {models.map((_, i) => (
            <Box
              key={i}
              w={i === 0 ? '30px' : '10px'}
              h="10px"
              bg={i === 0 ? 'white' : 'rgba(215,215,218,0.35)'}
              borderRadius="full"
              transition="all 0.3s ease"
            />
          ))}
        </Flex>

        {/* Play/Pause (decorative only) */}
        <Button
          onClick={() => setPaused((p) => !p)}
          rounded="full"
          p={3}
          bg="rgba(148,149,153,0.18)"
          _hover={{ bg: 'rgba(148,149,153,0.3)' }}
        >
          {paused ? (
            <img
              src="https://cdn.ui.porsche.com/porsche-design-system/icons/play.24226d4.svg"
              width="24"
              height="24"
              alt="Play"
            />
          ) : (
            <img
              src="https://cdn.ui.porsche.com/porsche-design-system/icons/pause.e41b935.svg"
              width="24"
              height="24"
              alt="Pause"
            />
          )}
        </Button>
      </Flex>
    </Box>
  );
}
