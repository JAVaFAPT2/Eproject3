import React, { useState, useEffect } from 'react';
import {
  Box,
  VStack,
  Text,
  Collapse,
  IconButton,
  Grid,
  useColorModeValue,
  Button,
  Flex,
  Image,
  useBreakpointValue,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import { MdExpandMore, MdClose, MdArrowBack } from 'react-icons/md';

import Clothe from 'assets/img/megamenu/clothe.png';
import Blog from 'assets/img/megamenu/blog.png';

const MotionIconButton = motion(IconButton);

export default function MegaMenu({ categories, onClose }) {
  const navbarBg = useColorModeValue('white', 'navy.800');
  const navbarBorder = useColorModeValue('rgba(11,20,55,0.1)', 'navy.600');

  const isMobile = useBreakpointValue({ base: true, md: false });

  const [expanded, setExpanded] = useState({});
  const [currentParentId, setCurrentParentId] = useState(null);

  useEffect(() => {
    document.body.style.overflow = 'hidden';
    return () => {
      document.body.style.overflow = 'auto';
    };
  }, []);

  const parents = categories.filter((c) => !c.parentId);
  const getChildren = (parentId) =>
    categories.filter((c) => c.parentId === parentId);

  const toggleExpand = (id) => {
    setExpanded((prev) => ({ ...prev, [id]: !prev[id] }));
  };

  return (
    <Box position="fixed" top="75px" left={0} w="100%" h="100vh">
      {/* Overlay làm mờ background */}
      <Box
        position="absolute"
        top={0}
        left={0}
        w="100%"
        h="100%"
        bg="rgba(0,0,0,0.4)"
        backdropFilter="blur(6px)"
      />

      {/* MegaMenu content */}
      <Box
        position="relative"
        w="100%"
        h={{ base: 'auto', md: '80vh' }}
        boxShadow="md"
        py={4}
        borderTop="1px solid"
        bg={navbarBg}
        borderBottom="1px solid"
        borderColor={navbarBorder}
        borderRadius="0 0 20px 20px"
        overflowY={{ base: 'visible', md: 'auto' }}
      >
        {/* Grid cho desktop, VStack cho mobile */}
        <Box maxW="1200px" mx="auto" px={6}>
          {isMobile ? (
            // Mobile
            <VStack align="start" spacing={2}>
              {currentParentId === null ? (
                parents.map((p, index) => (
                  <Flex
                    key={p.id}
                    justify="space-between"
                    align="center"
                    w="100%"
                    py={2}
                    borderBottom={
                      index !== parents.length - 1 ? '1px solid' : 'none'
                    }
                    borderColor={navbarBorder}
                    cursor="pointer"
                    onClick={() => {
                      if (getChildren(p.id).length > 0)
                        setCurrentParentId(p.id);
                      else console.log('Selected category', p.id);
                    }}
                  >
                    <Text fontWeight="bold">{p.name}</Text>
                    {getChildren(p.id).length > 0 && (
                      <MdArrowBack style={{ visibility: 'hidden' }} />
                    )}
                  </Flex>
                ))
              ) : (
                <>
                  <Flex
                    w="100%"
                    align="center"
                    position="relative"
                    cursor="pointer"
                    onClick={() => setCurrentParentId(null)}
                    py={2}
                    borderBottom="1px solid"
                    borderColor={navbarBorder}
                  >
                    {/* Icon arrow bên trái */}
                    <Box position="absolute" left={2}>
                      <MdArrowBack />
                    </Box>

                    {/* Tên category ở giữa */}
                    <Text fontWeight="bold" textAlign="center" w="100%">
                      {parents.find((p) => p.id === currentParentId)?.name ||
                        'Back'}
                    </Text>
                  </Flex>

                  {getChildren(currentParentId).map((c2) => (
                    <Box key={c2.id} w="100%">
                      <Flex
                        justify="space-between"
                        align="center"
                        cursor="pointer"
                        py={2}
                        onClick={() => toggleExpand(c2.id)}
                      >
                        <Text>{c2.name}</Text>
                        {getChildren(c2.id).length > 0 && (
                          <MotionIconButton
                            aria-label="expand"
                            icon={<MdExpandMore />}
                            size="sm"
                            variant="ghost"
                            animate={{ rotate: expanded[c2.id] ? 180 : 0 }}
                            transition={{ duration: 0.2 }}
                          />
                        )}
                      </Flex>
                      <Collapse in={expanded[c2.id]} animateOpacity>
                        <VStack align="start" pl={4} spacing={1} mt={1}>
                          {getChildren(c2.id).map((c3) => (
                            <Text
                              key={c3.id}
                              fontSize="sm"
                              cursor="pointer"
                              _hover={{ color: 'blue.500' }}
                            >
                              {c3.name}
                            </Text>
                          ))}
                        </VStack>
                      </Collapse>
                    </Box>
                  ))}
                </>
              )}
            </VStack>
          ) : (
            // Desktop
            <Grid
              templateColumns={`repeat(${parents.length + 1}, 1fr)`} // +1 for products/blog
              gap={8}
              display={{ base: 'none', md: 'grid' }}
            >
              {parents.map((p, idx) => (
                <VStack
                  key={p.id}
                  align="start"
                  spacing={3}
                  pr={4}
                  borderRight={idx !== parents.length ? '1px solid' : 'none'} // 👈 separator
                  borderColor={navbarBorder}
                >
                  <Text fontSize="lg" fontWeight="bold">
                    {p.name}
                  </Text>
                  {getChildren(p.id).map((c2) => (
                    <Box key={c2.id} w="100%">
                      <Flex
                        justify="space-between"
                        align="center"
                        cursor="pointer"
                        onClick={() => toggleExpand(c2.id)}
                      >
                        <Text>{c2.name}</Text>
                        <MotionIconButton
                          aria-label="expand"
                          icon={<MdExpandMore />}
                          size="sm"
                          variant="ghost"
                          animate={{ rotate: expanded[c2.id] ? 180 : 0 }}
                          transition={{ duration: 0.2 }}
                        />
                      </Flex>
                      <Collapse in={expanded[c2.id]} animateOpacity>
                        <VStack align="start" pl={4} spacing={1} mt={1}>
                          {getChildren(c2.id).map((c3) => (
                            <Text
                              key={c3.id}
                              fontSize="sm"
                              cursor="pointer"
                              _hover={{ color: 'blue.500' }}
                            >
                              {c3.name}
                            </Text>
                          ))}
                        </VStack>
                      </Collapse>
                    </Box>
                  ))}
                </VStack>
              ))}

              {/* Extra column for Products + Blog */}
              <VStack align="stretch" spacing={4} pl={4}>
                {/* Products Card */}
                <Box
                  borderRadius="lg"
                  shadow="md"
                  bg="white"
                  _dark={{ bg: 'navy.700' }}
                  cursor="pointer"
                  overflow="hidden"
                  _hover={{ shadow: 'lg', transform: 'translateY(-2px)' }}
                  transition="all 0.2s"
                >
                  <Image
                    src={Clothe}
                    alt="Products"
                    w="100%"
                    h="120px"
                    objectFit="cover"
                  />
                  <Box p={4}>
                    <Text fontSize="lg" fontWeight="bold" mb={2}>
                      Explore Products
                    </Text>
                    <Text fontSize="sm" color="gray.500">
                      Browse all categories, new arrivals, and bestsellers.
                    </Text>
                  </Box>
                </Box>

                {/* Blog Card */}
                <Box
                  borderRadius="lg"
                  shadow="md"
                  bg="white"
                  _dark={{ bg: 'navy.700' }}
                  cursor="pointer"
                  overflow="hidden"
                  _hover={{ shadow: 'lg', transform: 'translateY(-2px)' }}
                  transition="all 0.2s"
                >
                  <Image
                    src={Blog}
                    alt="Blog"
                    w="100%"
                    h="120px"
                    objectFit="cover"
                  />
                  <Box p={4}>
                    <Text fontSize="lg" fontWeight="bold" mb={2}>
                      Visit Our Blog
                    </Text>
                    <Text fontSize="sm" color="gray.500">
                      Get style tips, trends, and fashion news.
                    </Text>
                  </Box>
                </Box>
              </VStack>
            </Grid>
          )}
        </Box>
      </Box>

      {/* Nút đóng */}
      <Flex justify="center" mt={2}>
        <Button
          leftIcon={<MdClose />}
          colorScheme="gray"
          variant="solid"
          position="absolute"
          onClick={onClose}
        >
          Close
        </Button>
      </Flex>
    </Box>
  );
}
