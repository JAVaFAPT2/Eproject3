import React, { useState, useEffect, useMemo } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
  ModalCloseButton,
  Button,
  FormControl,
  FormLabel,
  Input,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Box,
  Text,
  Flex,
  useColorModeValue,
  IconButton,
  Divider,
  HStack,
} from '@chakra-ui/react';
import {
  ChevronDownIcon,
  AddIcon,
  DeleteIcon,
  RepeatIcon,
} from '@chakra-ui/icons';
import { useAppToast } from 'utils/ToastHelper';
import PurchaseOrderService from 'services/PurchaseOrderService';
import { useUser } from 'contexts/UserContext';
import { formatUSD } from 'utils/FormatHelper';

export default function PurchaseOrderForm({
  isOpen,
  onClose,
  reloadOrders,
  models,
}) {
  const toast = useAppToast();
  const { user } = useUser();
  const textColor = useColorModeValue('gray.800', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');

  const [orderLines, setOrderLines] = useState([
    { modelId: '', modelName: '', quantity: '', pricePerUnit: '' },
  ]);
  const [createdBy, setCreatedBy] = useState('');

  useEffect(() => {
    if (user) setCreatedBy(user.id);
  }, [user]);

  // ✅ Khi mở form: giữ line có model, bỏ line trống
  useEffect(() => {
    if (isOpen) {
      setOrderLines((prev) => {
        const withModel = prev.filter((line) => line.modelId);
        return withModel.length > 0
          ? withModel
          : [{ modelId: '', modelName: '', quantity: '', pricePerUnit: '' }];
      });
    }
  }, [isOpen]);

  const updateLine = (index, field, value) => {
    setOrderLines((prev) => {
      const updated = [...prev];
      updated[index][field] = value;
      return updated;
    });
  };

  const addLine = () => {
    setOrderLines((prev) => [
      ...prev,
      { modelId: '', modelName: '', quantity: '', pricePerUnit: '' },
    ]);
  };

  const resetLines = () => {
    setOrderLines([
      { modelId: '', modelName: '', quantity: '', pricePerUnit: '' },
    ]);
  };

  const removeLine = (index) => {
    setOrderLines((prev) => prev.filter((_, i) => i !== index));
  };

  // ✅ Tính tổng động
  const totalAmount = useMemo(() => {
    return orderLines.reduce((sum, line) => {
      const qty = Number(line.quantity) || 0;
      const price = Number(line.pricePerUnit) || 0;
      return sum + qty * price;
    }, 0);
  }, [orderLines]);

  // ✅ Submit: lọc dòng hợp lệ
  const handleSubmit = async () => {
    const validLines = orderLines.filter(
      (line) =>
        line.modelId &&
        Number(line.quantity) > 0 &&
        Number(line.pricePerUnit) > 0,
    );

    if (validLines.length === 0) {
      toast.error('Please fill at least one valid line before submitting.');
      return;
    }

    try {
      const orderRes = await PurchaseOrderService.create({
        createdBy,
        totalAmount,
      });

      const orderId = orderRes?.id;
      if (!orderId) throw new Error('No order ID returned.');

      for (const line of validLines) {
        await PurchaseOrderService.addLine(orderId, {
          modelId: line.modelId,
          quantity: Number(line.quantity),
          pricePerUnit: Number(line.pricePerUnit),
        });
      }

      toast.success('Purchase order created successfully');
      reloadOrders?.();
      onClose();
      resetLines();
    } catch (err) {
      console.error('❌ Error creating order:', err);
      toast.error('Failed to create purchase order');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="3xl" isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" maxH="85vh" overflow="hidden">
        <ModalHeader fontSize="2xl" fontWeight="700">
          Create Purchase Order
        </ModalHeader>
        <ModalCloseButton />

        <ModalBody overflowY="auto" maxH="60vh" px={6}>
          {/* Header row */}
          <Text fontSize="sm" color="gray.500" mb={3}>
            Created by:{' '}
            <Text as="span" fontWeight="500" color={textColor}>
              {user?.name || user?.username || 'Unknown'}
            </Text>
          </Text>

          {/* Scrollable order lines */}
          <Box minH="200px">
            <Flex direction="column" gap={4}>
              {orderLines.map((line, index) => (
                <Flex key={index} align="flex-end" gap={4} bg={bgColor}>
                  {/* Model */}
                  <FormControl isRequired flex="1.5">
                    <FormLabel fontSize="sm">Model</FormLabel>
                    <Menu isLazy matchWidth>
                      <MenuButton
                        as={Button}
                        rightIcon={<ChevronDownIcon />}
                        w="full"
                        variant="outline"
                      >
                        {line.modelName || 'Select model'}
                      </MenuButton>
                      <MenuList maxH="250px" overflowY="auto" bg={bgColor}>
                        {models.length > 0 ? (
                          models.map((m) => {
                            const isChild = m.level === 2;
                            return (
                              <MenuItem
                                key={m.modelNumber}
                                isDisabled={!isChild}
                                pl={m.level * 4}
                                fontWeight={m.level === 1 ? '700' : '500'}
                                opacity={m.level === 1 ? 0.8 : 1}
                                _hover={
                                  !isChild
                                    ? { bg: 'transparent', cursor: 'default' }
                                    : { bg: 'gray.100' }
                                }
                                onClick={() => {
                                  if (!isChild) return;
                                  updateLine(index, 'modelId', m.modelNumber);
                                  updateLine(index, 'modelName', m.name);
                                }}
                              >
                                {m.level === 1 ? m.name : `${m.name}`}
                              </MenuItem>
                            );
                          })
                        ) : (
                          <Box px={3} py={2}>
                            <Text fontSize="sm" color="gray.500">
                              No models available
                            </Text>
                          </Box>
                        )}
                      </MenuList>
                    </Menu>
                  </FormControl>

                  {/* Quantity */}
                  <FormControl isRequired flex="1">
                    <FormLabel fontSize="sm">Quantity</FormLabel>
                    <Input
                      type="number"
                      value={line.quantity}
                      onChange={(e) =>
                        updateLine(index, 'quantity', e.target.value)
                      }
                    />
                  </FormControl>

                  {/* Price */}
                  <FormControl isRequired flex="1">
                    <FormLabel fontSize="sm">Price</FormLabel>
                    <Input
                      type="number"
                      value={line.pricePerUnit}
                      onChange={(e) =>
                        updateLine(index, 'pricePerUnit', e.target.value)
                      }
                    />
                  </FormControl>

                  {/* Delete */}
                  {orderLines.length > 1 && (
                    <IconButton
                      aria-label="Delete"
                      icon={<DeleteIcon />}
                      colorScheme="red"
                      variant="ghost"
                      size="sm"
                      mb="2"
                      onClick={() => removeLine(index)}
                    />
                  )}
                </Flex>
              ))}
            </Flex>
          </Box>

          <Divider my={5} />

          {/* ✅ Row: Add + Reset + Total */}
          <Flex justify="space-between" align="center">
            <HStack spacing={3}>
              <Button
                leftIcon={<AddIcon />}
                colorScheme="blue"
                variant="solid"
                onClick={addLine}
                size="sm"
              >
                Add Model
              </Button>

              <Button
                leftIcon={<RepeatIcon />}
                colorScheme="gray"
                variant="outline"
                onClick={resetLines}
                size="sm"
              >
                Reset
              </Button>
            </HStack>

            <Text fontWeight="700" fontSize="lg" color={textColor}>
              Total:{' '}
              <Text as="span" color="green.500">
                {formatUSD(totalAmount)}
              </Text>
            </Text>
          </Flex>
        </ModalBody>

        <ModalFooter borderTopWidth="1px">
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button colorScheme="green" onClick={handleSubmit}>
            Order
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
