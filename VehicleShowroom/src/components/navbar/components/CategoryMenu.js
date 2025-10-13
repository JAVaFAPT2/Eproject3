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
import { CloseIcon } from '@chakra-ui/icons';
import { NavLink, useNavigate } from 'react-router-dom';
import VehicleModelService from 'services/VehicleModelService';
import { MdLogin } from 'react-icons/md';

const MotionBox = motion(Box);
const MotionImage = motion(Image);

export default function CategoryMenu({ isVisible, closeHandler }) {
  const [models, setModels] = useState([]);
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

  const handleSignInClick = () => {
    closeHandler();
    navigate('/auth/sign-in');
  };

  return (
    <AnimatePresence>
      {isVisible && (
        <>
          {/* ✅ Overlay */}
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

          {/* ✅ Drawer */}
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
            {/* 🔹 Scrollable content */}
            <Box
              flex="1"
              overflowY="auto"
              px={{ base: 6, md: 10 }}
              py={{ base: 4, md: 8 }}
            >
              <Flex justify="space-between" mb={6}>
                <Text
                  fontSize={{ base: '2xl', md: '3xl' }}
                  fontWeight="semibold"
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

              {/* 🔹 Loading & error states */}
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

              {/* 🔹 List of models (1 per row) */}
              {!loading && !error && (
                <Grid templateColumns="1fr" gap={6} pb={10}>
                  {models.map((m) => (
                    <MotionBox
                      key={m.modelNumber}
                      borderRadius="xl"
                      overflow="hidden"
                      p={4}
                      transition="all 0.25s ease"
                      _hover={{ backgroundColor: 'white' }}
                    >
                      <NavLink
                        to={`/user/models/${m.modelNumber}`}
                        onClick={closeHandler}
                      >
                        <Text
                          fontSize="2xl"
                          fontWeight="semibold"
                          mb={3}
                          _hover={{ textDecoration: 'underline' }}
                        >
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
                            transition={{
                              duration: 0.2,
                              ease: 'easeOut',
                            }}
                            whileHover={{ x: 8 }}
                          />
                        </Box>
                      </NavLink>
                    </MotionBox>
                  ))}
                </Grid>
              )}
            </Box>

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
