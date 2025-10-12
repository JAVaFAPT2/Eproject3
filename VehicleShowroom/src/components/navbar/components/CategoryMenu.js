import React, { useEffect, useState } from 'react';
import {
  Box,
  Flex,
  Grid,
  Image,
  Text,
  Tag,
  IconButton,
  useColorModeValue,
} from '@chakra-ui/react';
import { motion, AnimatePresence } from 'framer-motion';
import { CloseIcon } from '@chakra-ui/icons';
import { NavLink } from 'react-router-dom';

const MotionBox = motion(Box);

export default function CategoryMenu({ isVisible, closeHandler }) {
  const [cars, setCars] = useState([]);
  const bg = useColorModeValue('whiteAlpha.900', 'gray.800');

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch('/JSON/models.json');
        const data = await response.json();
        setCars(data);
      } catch (error) {
        console.error('Error fetching car models:', error);
      }
    }
    fetchData();
  }, []);

  return (
    <AnimatePresence>
      {isVisible && (
        <>
          {/* ✅ Backdrop overlay */}
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

          {/* ✅ Drawer content */}
          <MotionBox
            position="fixed"
            top="0"
            left="0"
            h="100vh"
            w={{ base: '100%', md: '80%', lg: '70%', xl: '65%' }}
            bg={bg}
            backdropFilter="blur(24px)"
            overflowY="auto"
            px={{ base: 6, md: 10 }}
            py={{ base: 4, md: 8 }}
            zIndex={1000}
            borderRightWidth="1px"
            initial={{ x: '-100%', opacity: 0 }}
            animate={{ x: 0, opacity: 1 }}
            exit={{ x: '-100%', opacity: 0 }}
            transition={{ duration: 0.45, ease: [0.075, 0.82, 0.165, 1] }}
          >
            {/* 🔹 Close button */}
            <Flex justify="space-between" mb={6}>
              <Text
                fontSize={{ base: '2xl', md: '3xl' }}
                fontWeight="semibold"
                mb={6}
                pl={2}
              >
                Explore Models
              </Text>

              <IconButton
                aria-label="Close menu"
                variant="ghost"
                icon={<CloseIcon boxSize={5} />}
                onClick={closeHandler}
                _hover={{ bg: 'blackAlpha.100' }}
              />
            </Flex>

            {/* 🔹 Grid of cars */}
            <Grid
              templateColumns={{
                base: '1fr',
                sm: 'repeat(2, 1fr)',
                md: 'repeat(3, 1fr)',
                lg: 'repeat(3, 1fr)',
              }}
              gap={6}
              pb={10}
            >
              {cars.map((car) => (
                <MotionBox
                  key={car.id}
                  borderRadius="xl"
                  overflow="hidden"
                  p={4}
                  shadow="md"
                  transition="all 0.25s ease"
                  whileHover={{ y: -6 }}
                >
                  <NavLink to={car.a || '#'} onClick={closeHandler}>
                    <Text
                      fontSize="lg"
                      fontWeight="semibold"
                      mb={3}
                      _hover={{ textDecoration: 'underline' }}
                    >
                      {car.model}
                    </Text>

                    <Box
                      position="relative"
                      overflow="hidden"
                      borderRadius="md"
                      mb={3}
                    >
                      <Image
                        src={car.image}
                        alt={car.model}
                        objectFit="cover"
                        w="100%"
                        transition="transform 0.3s ease"
                        _hover={{ transform: 'scale(1.05)' }}
                      />
                    </Box>
                  </NavLink>

                  <Flex mt={2} flexWrap="wrap" gap={2}>
                    <Tag colorScheme="gray" fontSize="sm" px={3} py={1}>
                      {car.type}
                    </Tag>
                    {car.type2 && (
                      <Tag colorScheme="gray" fontSize="sm" px={3} py={1}>
                        {car.type2}
                      </Tag>
                    )}
                  </Flex>
                </MotionBox>
              ))}
            </Grid>
          </MotionBox>
        </>
      )}
    </AnimatePresence>
  );
}
