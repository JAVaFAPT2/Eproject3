import React, { useState, useEffect } from 'react';
import {
  Box,
  Text,
  Flex,
  Stack,
  Badge,
  Button,
  Image,
  useDisclosure,
  useColorModeValue,
  Spinner,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import ServiceForm from './ServiceForm';
import ServiceOrderService from 'services/ServiceOrderService';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';
import { useAppToast } from 'utils/ToastHelper';

const ORDER_STATUS_MAP = {
  1: 'Pending',
  2: 'Confirmed',
  3: 'Completed',
  4: 'Cancelled',
};

const STATUS_COLOR = {
  1: 'yellow',
  2: 'blue',
  3: 'green',
  4: 'red',
};

const MotionBox = motion(Box);

export default function OrderHistoryTab({ orders }) {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [modelMap, setModelMap] = useState({});
  const [loading, setLoading] = useState(false);
  const toast = useAppToast();

  const bgCard = useColorModeValue('white', 'navy.700');
  const borderColor = useColorModeValue('gray.200', 'gray.600');

  // 🧠 Fetch model + photo for each order
  useEffect(() => {
    if (!orders || orders.length === 0) return;

    const fetchModels = async () => {
      try {
        setLoading(true);

        const modelData = {};
        await Promise.all(
          orders.map(async (order) => {
            try {
              const modelRes = await VehicleModelService.get({
                modelNumber: order.modelNumber,
              });

              const model =
                Array.isArray(modelRes?.items) && modelRes.items.length > 0
                  ? modelRes.items[0]
                  : modelRes;

              const photos = await VehiclePhotoService.getByModelNumber(
                order.modelNumber,
              );

              modelData[order.modelNumber] = {
                ...model,
                photoUrl: photos[0]?.photoUrl || '',
              };
            } catch (err) {
              console.warn(`⚠️ Failed to fetch model for ${order.modelNumber}`);
            }
          }),
        );

        setModelMap(modelData);
      } catch (err) {
        console.error('❌ Failed to load models:', err);
        toast.error('Failed to load model information');
      } finally {
        setLoading(false);
      }
    };

    fetchModels();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [orders]);

  const handleCreateService = (order) => {
    setSelectedOrder(order);
    onOpen();
  };

  const handleSubmitService = async (data) => {
    try {
      await ServiceOrderService.create({
        orderId: selectedOrder.id,
        customerId: selectedOrder.customerId,
        createdBy: selectedOrder.customerId,
        type: data.type,
        description: data.description,
      });

      toast.success('Service order created successfully');
      onClose();
    } catch (err) {
      console.error('❌ Create service failed:', err);
      toast.error('Failed to create service order');
    }
  };

  if (!orders || orders.length === 0)
    return <Text>No order history found.</Text>;

  return (
    <Box>
      <Text fontSize="xl" fontWeight="bold" mb={4}>
        Order History
      </Text>

      {loading && (
        <Flex justify="center" align="center" py={4}>
          <Spinner />
        </Flex>
      )}

      <Stack spacing={5}>
        {orders.map((order) => {
          const modelInfo = modelMap[order.modelNumber];
          return (
            <MotionBox
              key={order.id}
              p={5}
              borderWidth="1px"
              borderColor={borderColor}
              borderRadius="lg"
              bg={bgCard}
              shadow="sm"
              whileHover={{ scale: 1.01 }}
              transition="0.2s"
            >
              <Flex gap={4} align="center">
                {/* Thumbnail */}
                {modelInfo?.photoUrl ? (
                  <Image
                    src={modelInfo.photoUrl}
                    alt={modelInfo.modelName || 'Model'}
                    boxSize="100px"
                    borderRadius="md"
                    objectFit="cover"
                  />
                ) : (
                  <Box
                    w="100px"
                    h="100px"
                    bg="gray.100"
                    borderRadius="md"
                    display="flex"
                    alignItems="center"
                    justifyContent="center"
                    fontSize="sm"
                    color="gray.500"
                  >
                    No Image
                  </Box>
                )}

                {/* Info */}
                <Box flex="1">
                  <Flex justify="space-between" align="center" mb={2}>
                    <Text fontWeight="600">#{order.id}</Text>
                    <Badge colorScheme={STATUS_COLOR[order.status] || 'gray'}>
                      {ORDER_STATUS_MAP[order.status] || 'Unknown'}
                    </Badge>
                  </Flex>

                  <Text fontSize="md">
                    <strong>Model:</strong>{' '}
                    {modelInfo?.modelName || order.modelNumber}
                  </Text>
                  <Text fontSize="md" mb={3}>
                    <strong>Price:</strong> ${order.salePrice?.toLocaleString()}
                  </Text>

                  {Number(order.status) === 3 && (
                    <Button
                      colorScheme="green"
                      size="sm"
                      onClick={() => handleCreateService(order)}
                    >
                      Create Service
                    </Button>
                  )}
                </Box>
              </Flex>
            </MotionBox>
          );
        })}
      </Stack>

      {selectedOrder && (
        <ServiceForm
          isOpen={isOpen}
          onClose={onClose}
          order={selectedOrder}
          onSubmit={handleSubmitService}
        />
      )}
    </Box>
  );
}
