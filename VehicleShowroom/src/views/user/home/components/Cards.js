import React, { useEffect, useState } from 'react';
import {
  Box,
  Container,
  Grid,
  Image,
  Text,
  Flex,
  Icon,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import { NavLink } from 'react-router-dom';
import { ArrowForwardIcon } from '@chakra-ui/icons';

const MotionBox = motion(Box);

export default function Cards() {
  const [cards, setCards] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const res = await fetch('/JSON/cards.json');
        const data = await res.json();
        setCards(data);
      } catch (error) {
        console.warn('Error loading cards:', error);
      }
    }
    fetchData();
  }, []);

  return (
    <Box
      mt={{ base: 12, md: 20 }}
      pb={{ base: 8, md: 16 }}
      w="100%"
      display="flex"
      justifyContent="center"
      alignItems="center"
    >
      <Container maxW="8xl" px={{ base: 4, md: 10 }}>
        <Grid
          templateColumns={{
            base: '1fr',
            sm: 'repeat(2, 1fr)',
            lg: 'repeat(3, 1fr)',
          }}
          gap={{ base: 6, md: 8 }}
        >
          {cards.map((el, idx) => (
            <MotionBox
              key={el.id || idx}
              position="relative"
              borderRadius="lg"
              overflow="hidden"
              cursor="pointer"
              initial={{ opacity: 0, y: 40 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{
                duration: 0.6,
                delay: idx * 0.15,
                ease: 'easeOut',
              }}
              whileHover={{ scale: 1.03 }}
            >
              <NavLink to={el.a || '#'}>
                {/* Ảnh */}
                <Image
                  src={el.src}
                  srcSet={el.srcSet}
                  sizes={el.sizes}
                  alt={el.alt}
                  w="100%"
                  h="100%"
                  objectFit="cover"
                  borderRadius="lg"
                  transition="transform 0.5s ease"
                  _hover={{ transform: 'scale(1.05)' }}
                />

                {/* Gradient overlay */}
                <Box
                  position="absolute"
                  bottom="0"
                  left="0"
                  w="100%"
                  h="40%"
                  bgGradient="linear(to-t, rgba(0,0,0,0.85), transparent)"
                  zIndex={1}
                  borderBottomRadius="lg"
                />

                {/* Text + arrow */}
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
                    fontWeight="600"
                    fontSize={{ base: 'md', md: 'lg' }}
                    maxW="80%"
                    noOfLines={2}
                  >
                    {el.name}
                  </Text>
                  <Icon as={ArrowForwardIcon} boxSize={6} color="white" />
                </Flex>
              </NavLink>
            </MotionBox>
          ))}
        </Grid>
      </Container>
    </Box>
  );
}
