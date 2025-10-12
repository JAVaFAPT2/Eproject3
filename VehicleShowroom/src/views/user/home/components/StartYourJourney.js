import React, { useEffect, useState } from 'react';
import {
  Box,
  Heading,
  Text,
  Flex,
  Button,
  Icon,
  HStack,
  Container,
  useColorModeValue,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import { ArrowForwardIcon } from '@chakra-ui/icons';
import { useNavigate } from 'react-router-dom';

const MotionBox = motion(Box);

export default function StartYourJourney() {
  const navigate = useNavigate();
  const [articles, setArticles] = useState([]);
  const [paused, setPaused] = useState(false);
  const bg = useColorModeValue('white', 'navy.900');

  // fetch JSON data
  useEffect(() => {
    async function fetchData() {
      try {
        const res = await fetch('/JSON/carVideos.json');
        const data = await res.json();
        setArticles(data);
      } catch (err) {
        console.error('Error fetching car videos:', err);
      } finally {
      }
    }
    fetchData();
  }, []);

  return (
    <Box
      bg={bg}
      py={{ base: 20, md: 40 }}
      maxW="1880px"
      mx="auto"
      position="relative"
    >
      {/* Header */}
      <Container maxW="6xl" textAlign="center" mb={{ base: 10, md: 20 }}>
        <Heading
          as="h2"
          fontSize={{ base: '3xl', md: '6xl' }}
          fontWeight="400"
          lineHeight="shorter"
          mb={4}
        >
          Your Porsche journey starts now.
        </Heading>
      </Container>

      {/* Video Articles */}
      <Flex
        overflowX="auto"
        gap={6}
        px={{ base: 4, md: 10 }}
        pb={8}
        justify="center"
        flexWrap={{ base: 'nowrap', md: 'wrap' }}
      >
        {articles.map((el, idx) => (
          <MotionBox
            key={el.id}
            flex={{ base: '0 0 85%', md: '1 1 45%' }}
            borderRadius="xl"
            overflow="hidden"
            cursor="pointer"
            position="relative"
            bg="black"
            onClick={() => navigate(`/models/${el.id}`)}
            initial={{ opacity: 0, y: 40 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{
              duration: 0.6,
              delay: idx * 0.1,
              ease: 'easeOut',
            }}
          >
            {/* Masked logo */}
            <Box
              position="absolute"
              top={4}
              left={4}
              w={{ base: '100px', md: '140px' }}
              h="auto"
              bg="white"
              maskImage={`url(${el.iconMask})`}
              maskSize="contain"
              maskRepeat="no-repeat"
              maskPosition="center"
              zIndex={2}
            />

            {/* Video */}
            <Box
              position="relative"
              h={{ base: '75vh', md: '45vh' }}
              overflow="hidden"
              borderRadius="xl"
            >
              <video
                preload="auto"
                src={el.video}
                poster={el.poster}
                autoPlay
                muted
                loop
                playsInline
                style={{
                  width: '100%',
                  height: '100%',
                  objectFit: 'cover',
                }}
              />
              {/* Gradient overlay */}
              <Box
                position="absolute"
                bottom={0}
                left={0}
                right={0}
                h="50%"
                bgGradient="linear(to-t, rgba(0,0,0,0.85), transparent)"
                borderBottomRadius="xl"
              />

              {/* Text content */}
              <Box
                position="absolute"
                bottom={0}
                left={0}
                w="100%"
                color="white"
                px={6}
                py={4}
              >
                {el.tag && (
                  <HStack mb={2}>
                    <Box
                      bg="rgba(255,255,255,0.25)"
                      backdropFilter="blur(12px)"
                      borderRadius="md"
                      px={3}
                      py={0.5}
                      fontSize="xs"
                    >
                      {el.tag}
                    </Box>
                  </HStack>
                )}
                <Text fontSize={{ base: 'md', md: 'lg' }} mb={2}>
                  {el.abouttext}
                </Text>
                <Flex align="center" gap={2}>
                  <Text fontWeight="500">Explore</Text>
                  <Icon as={ArrowForwardIcon} boxSize={5} />
                </Flex>
              </Box>
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
          {[...Array(6)].map((_, i) => (
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

        {/* Play/Pause button */}
        <Button
          onClick={() => setPaused((p) => !p)}
          rounded="full"
          p={3}
          bg="rgba(148,149,153,0.18)"
          _hover={{ bg: 'rgba(148,149,153,0.3)' }}
        >
          <Icon
            as={paused ? ArrowForwardIcon : undefined}
            boxSize={6}
            color="white"
          />
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
