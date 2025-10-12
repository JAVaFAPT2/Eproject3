import React, { useEffect, useState } from 'react';
import {
  Box,
  Container,
  Grid,
  Heading,
  Text,
  Flex,
  Image,
  Icon,
  useColorModeValue,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import { ArrowForwardIcon } from '@chakra-ui/icons';

const MotionBox = motion(Box);

export default function Discover() {
  const [items, setItems] = useState([]);
  const bg = useColorModeValue('white', 'navy.800');

  useEffect(() => {
    async function fetchData() {
      try {
        const res = await fetch('/JSON/discover.json');
        const data = await res.json();
        setItems(data);
      } catch (err) {
        console.error('Error fetching discover items:', err);
      }
    }
    fetchData();
  }, []);

  return (
    <Box bg={bg} py={{ base: 16, md: 28 }}>
      {/* Header */}
      <Container maxW="6xl" textAlign="center" mb={{ base: 12, md: 20 }}>
        <Heading
          as="h2"
          fontSize={{ base: '3xl', md: '6xl' }}
          fontWeight="600"
          letterSpacing="tight"
        >
          Discover
        </Heading>
      </Container>

      {/* Grid items */}
      <Container maxW="8xl" px={{ base: 4, md: 8 }}>
        <Grid
          templateColumns={{
            base: '1fr',
            md: 'repeat(2, 1fr)',
            xl: 'repeat(3, 1fr)',
          }}
          gap={{ base: 6, md: 8 }}
        >
          {items.map((el, i) => (
            <MotionBox
              key={i}
              position="relative"
              borderRadius="xl"
              overflow="hidden"
              cursor="pointer"
              initial={{ opacity: 0, y: 30 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{
                duration: 0.6,
                delay: i * 0.1,
                ease: 'easeOut',
              }}
              whileHover={{ scale: 1.02 }}
            >
              {/* Image */}
              <Box position="relative" overflow="hidden">
                <picture>
                  <source media="(max-width: 479px)" srcSet={el.source1} />
                  <source media="(max-width: 759px)" srcSet={el.source2} />
                  <source media="(max-width: 999px)" srcSet={el.source3} />
                  <source media="(max-width: 1299px)" srcSet={el.source4} />
                  <source media="(max-width: 1759px)" srcSet={el.source5} />
                  <source media="(max-width: 1919px)" srcSet={el.source6} />
                  <source media="(min-width: 1920px)" srcSet={el.source7} />
                  <Image
                    src={el.img}
                    alt={el.alt}
                    w="100%"
                    h="100%"
                    objectFit="cover"
                    transition="transform 0.5s ease"
                    _hover={{ transform: 'scale(1.08)' }}
                  />
                </picture>

                {/* Gradient overlay */}
                <Box
                  position="absolute"
                  bottom="0"
                  left="0"
                  right="0"
                  h="50%"
                  bgGradient="linear(to-t, rgba(0,0,0,0.85), transparent)"
                  zIndex={1}
                />

                {/* Footer text */}
                <Flex
                  position="absolute"
                  bottom="0"
                  left="0"
                  w="100%"
                  px={6}
                  py={4}
                  justify="space-between"
                  align="center"
                  zIndex={2}
                >
                  <Text
                    color="white"
                    fontSize={{ base: 'lg', md: 'xl' }}
                    fontWeight="500"
                    noOfLines={1}
                  >
                    {el.name}
                  </Text>
                  <Icon
                    as={ArrowForwardIcon}
                    boxSize={6}
                    color="white"
                    opacity={0.9}
                  />
                </Flex>
              </Box>
            </MotionBox>
          ))}
        </Grid>
      </Container>
    </Box>
  );
}
