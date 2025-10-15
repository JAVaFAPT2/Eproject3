import React, { useEffect, useState } from 'react';
import {
  Box,
  Grid,
  GridItem,
  Image,
  Heading,
  Flex,
  Tag,
  VStack,
  Spinner,
  IconButton,
  Text,
  useColorModeValue,
} from '@chakra-ui/react';
import { ArrowBackIcon } from '@chakra-ui/icons';
import { NavLink } from 'react-router-dom';
import VehicleModelService from 'services/VehicleModelService';

function IndividualCars() {
  const [models, setModels] = useState([]);
  const [parentModel, setParentModel] = useState(null);
  const [loading, setLoading] = useState(true);
  const bgItem = useColorModeValue('#eeeff2', 'gray.700');
  const borderHover = useColorModeValue('gray.300', 'gray.500');

  // ✅ Load model cấp 1
  const loadLevel1 = async () => {
    setLoading(true);
    try {
      const data = await VehicleModelService.search({
        pageNumber: 1,
        pageSize: 50,
      });
      setModels(data?.items || data || []);
    } catch (error) {
      console.error('Failed to load models:', error);
    } finally {
      setLoading(false);
    }
  };

  // ✅ Load model cấp 2 (theo modelNumber cha)
  const loadLevel2 = async (parentModelNumber, name) => {
    setLoading(true);
    try {
      const data = await VehicleModelService.search({
        parentModelNumber,
        pageNumber: 1,
        pageSize: 50,
      });
      setModels(data?.items || data || []);
      setParentModel({ modelNumber: parentModelNumber, name });
    } catch (error) {
      console.error('Failed to load variants:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadLevel1();
  }, []);

  const handleBack = () => {
    setParentModel(null);
    loadLevel1();
  };

  if (loading)
    return (
      <Flex justify="center" align="center" h="100%">
        <Spinner size="lg" />
      </Flex>
    );

  return (
    <Box>
      {/* 🔹 Nút Back khi đang ở cấp 2 */}
      {parentModel && (
        <Flex align="center" mb={4}>
          <IconButton
            icon={<ArrowBackIcon />}
            aria-label="Back"
            variant="ghost"
            mr={2}
            onClick={handleBack}
          />
          <Heading size="md">{parentModel.name}</Heading>
        </Flex>
      )}

      <Grid templateColumns="1fr" gap={6} placeItems="center" py={2} pb={8}>
        {models.length === 0 ? (
          <Text color="gray.500" fontStyle="italic">
            No models found
          </Text>
        ) : (
          models.map((el) => (
            <GridItem
              key={el.modelNumber}
              w="full"
              maxW="22rem"
              bg={bgItem}
              borderRadius="md"
              p={4}
              transition="0.25s ease"
              _hover={{ border: '1px solid', borderColor: borderHover }}
            >
              <VStack align="start" spacing={3}>
                <Heading
                  size="md"
                  fontWeight="semibold"
                  cursor="pointer"
                  onClick={() =>
                    el.level === 1 ? loadLevel2(el.modelNumber, el.name) : null
                  }
                >
                  {el.name}
                </Heading>

                <Box
                  position="relative"
                  w="full"
                  overflow="hidden"
                  borderRadius="md"
                >
                  <NavLink
                    to={
                      el.slug && el.level === 2 ? `/user/model/${el.slug}` : '#'
                    }
                    onClick={(e) => {
                      if (el.level === 1) {
                        e.preventDefault();
                        loadLevel2(el.modelNumber, el.name);
                      }
                    }}
                  >
                    <Image
                      src={
                        el.photo || 'https://placehold.co/600x400?text=No+Image'
                      }
                      alt={el.name}
                      w="full"
                      h="auto"
                      objectFit="cover"
                      transition="transform 0.3s ease"
                      _hover={{ transform: 'translateX(10px)' }}
                      borderRadius="md"
                    />
                  </NavLink>
                </Box>

                <Flex gap={2} wrap="wrap">
                  <Tag bg="white" color="black" fontWeight="medium">
                    {el.price ? `$${el.price.toLocaleString()}` : 'N/A'}
                  </Tag>
                  {el.level === 2 && (
                    <Tag bg="gray.200" color="black" fontWeight="medium">
                      Variant
                    </Tag>
                  )}
                </Flex>
              </VStack>
            </GridItem>
          ))
        )}
      </Grid>
    </Box>
  );
}

export default IndividualCars;
