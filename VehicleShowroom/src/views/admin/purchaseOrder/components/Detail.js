import React, { useEffect, useState } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  Flex,
  Text,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  Badge,
  Spinner,
  Button,
  useColorModeValue,
} from '@chakra-ui/react';
import PurchaseOrderService from 'services/PurchaseOrderService';
import { formatUSD } from 'utils/FormatHelper';

// ✅ Enum map
const STATUS_MAP = {
  1: { label: 'Pending', color: 'yellow' },
  2: { label: 'Completed', color: 'green' },
  3: { label: 'Cancelled', color: 'red' },
};

export default function Detail({ isOpen, onClose, orderId }) {
  const [detail, setDetail] = useState(null);
  const [loading, setLoading] = useState(false);
  const bg = useColorModeValue('white', 'navy.800');

  useEffect(() => {
    if (!isOpen || !orderId) return;

    const loadDetail = async () => {
      setLoading(true);
      try {
        const res = await PurchaseOrderService.getById(orderId);
        setDetail(res);
      } catch (err) {
        console.error('Failed to load order detail:', err);
      } finally {
        setLoading(false);
      }
    };

    loadDetail();
  }, [isOpen, orderId]);

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="4xl" isCentered>
      <ModalOverlay />
      <ModalContent bg={bg} minH="600px">
        <ModalHeader>Purchase Order Detail</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          {loading ? (
            <Flex justify="center" py={10}>
              <Spinner />
            </Flex>
          ) : !detail ? (
            <Text>No data available.</Text>
          ) : (
            <>
              {/* 🔹 Thông tin chung */}
              <Flex justify="space-between" mb={4} flexWrap="wrap" gap={4}>
                <Flex direction="column">
                  <Text fontWeight="bold">Order ID:</Text>
                  <Text>{detail.id}</Text>
                </Flex>

                <Flex direction="column">
                  <Text fontWeight="bold">Order Date:</Text>
                  <Text>
                    {new Date(detail.orderDate).toLocaleString('en-US', {
                      day: '2-digit',
                      month: '2-digit',
                      year: 'numeric',
                      hour: '2-digit',
                      minute: '2-digit',
                    })}
                  </Text>
                </Flex>

                <Flex direction="column">
                  <Text fontWeight="bold">Total Amount:</Text>
                  <Text color="blue.500" fontWeight="bold">
                    {formatUSD(detail.totalAmount)}
                  </Text>
                </Flex>

                <Flex direction="column">
                  <Text fontWeight="bold">Status:</Text>
                  <Badge
                    colorScheme={STATUS_MAP[detail.status]?.color || 'gray'}
                    px={3}
                    py={1}
                    borderRadius="md"
                  >
                    {STATUS_MAP[detail.status]?.label || 'Unknown'}
                  </Badge>
                </Flex>
              </Flex>

              {/* 🔹 Bảng chi tiết dòng hàng */}
              <Table variant="simple">
                <Thead>
                  <Tr>
                    <Th>#</Th>
                    <Th>Model ID</Th>
                    <Th isNumeric>Quantity</Th>
                    <Th isNumeric>Price/Unit</Th>
                    <Th isNumeric>Line Total</Th>
                  </Tr>
                </Thead>
                <Tbody>
                  {detail.lines?.length > 0 ? (
                    detail.lines.map((line, i) => (
                      <Tr key={line.id}>
                        <Td>{i + 1}</Td>
                        <Td>{line.modelId}</Td>
                        <Td isNumeric>{line.quantity}</Td>
                        <Td isNumeric>{formatUSD(line.pricePerUnit)}</Td>
                        <Td isNumeric color="blue.400" fontWeight="bold">
                          {formatUSD(line.lineTotal)}
                        </Td>
                      </Tr>
                    ))
                  ) : (
                    <Tr>
                      <Td colSpan={5}>
                        <Text textAlign="center" py={3}>
                          No lines available.
                        </Text>
                      </Td>
                    </Tr>
                  )}
                </Tbody>
              </Table>
            </>
          )}
        </ModalBody>

        <ModalFooter>
          <Button variant="ghost" onClick={onClose}>
            Close
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
