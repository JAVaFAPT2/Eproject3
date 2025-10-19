import React, { useEffect, useState } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  Button,
  Table,
  Thead,
  Tr,
  Th,
  Tbody,
  Td,
  useColorModeValue,
  Spinner,
  Flex,
  Text,
} from '@chakra-ui/react';
import VehicleService from 'services/VehicleService';
import ConfirmDialog from 'components/dialog/ConfirmDialog'; // ✅ import dialog

export default function AssignForm({
  isOpen,
  onClose,
  order,
  onAssigned,
  onCancelled,
}) {
  const [vehicles, setVehicles] = useState([]);
  const [selectedVehicle, setSelectedVehicle] = useState(null);
  const [assignedVehicle, setAssignedVehicle] = useState(null);
  const [loading, setLoading] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false); // ✅ state cho dialog

  const bg = useColorModeValue('white', 'navy.800');

  useEffect(() => {
    if (!isOpen || !order) return;

    const load = async () => {
      setLoading(true);
      try {
        if (order.vehicleId) {
          const res = await VehicleService.get({ id: order.vehicleId });
          const vehicle =
            res.vehicle ||
            res.items?.find((v) => v.vehicleId === order.vehicleId);
          setAssignedVehicle(vehicle || null);
          setVehicles([]);
        } else {
          const res = await VehicleService.get({
            pageNumber: 1,
            pageSize: 50,
            modelNumber: order.modelNumber,
            status: 1, // In Stock
          });
          setVehicles(res.items || []);
          setAssignedVehicle(null);
        }
      } catch (err) {
        console.error('Failed to load vehicles', err);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [isOpen, order]);

  const handleAssign = async () => {
    if (!selectedVehicle) return;
    await onAssigned(selectedVehicle);
    setSelectedVehicle(null);
    onClose();
  };

  const handleCancelClick = () => setConfirmOpen(true); // mở dialog xác nhận

  const confirmCancel = async () => {
    if (!order) return;
    await onCancelled(order);
    setConfirmOpen(false);
    onClose();
  };

  return (
    <>
      {/* 🔹 Dialog xác nhận */}
      <ConfirmDialog
        isOpen={confirmOpen}
        onClose={() => setConfirmOpen(false)}
        onConfirm={confirmCancel}
        title="Cancel Order"
        message={`Are you sure you want to cancel this order for ${
          order?.customerName || 'this customer'
        }?`}
      />

      <Modal isOpen={isOpen} onClose={onClose} size="4xl" isCentered>
        <ModalOverlay />
        <ModalContent bg={bg} minH="600px">
          <ModalHeader>
            {order?.vehicleId
              ? `Assigned Vehicle for ${order?.customerName}'s Order`
              : `Assign Vehicle for ${order?.customerName}'s Order`}
          </ModalHeader>
          <ModalCloseButton />
          <ModalBody>
            {loading ? (
              <Flex justify="center" py={10}>
                <Spinner />
              </Flex>
            ) : assignedVehicle ? (
              <Table variant="simple">
                <Thead>
                  <Tr>
                    <Th>#</Th>
                    <Th>Vehicle ID</Th>
                    <Th>Model</Th>
                    <Th>Status</Th>
                  </Tr>
                </Thead>
                <Tbody>
                  <Tr>
                    <Td>1</Td>
                    <Td>{assignedVehicle.vehicleId}</Td>
                    <Td>{assignedVehicle.modelNumber}</Td>
                    <Td>Reserved</Td>
                  </Tr>
                </Tbody>
              </Table>
            ) : vehicles.length === 0 ? (
              <Text>No vehicles available in stock.</Text>
            ) : (
              <Table variant="simple">
                <Thead>
                  <Tr>
                    <Th>#</Th>
                    <Th>Vehicle ID</Th>
                    <Th>Model</Th>
                    <Th>Status</Th>
                    <Th textAlign="right">Select</Th>
                  </Tr>
                </Thead>
                <Tbody>
                  {vehicles.map((v, i) => (
                    <Tr key={v.vehicleId}>
                      <Td>{i + 1}</Td>
                      <Td>{v.vehicleId}</Td>
                      <Td>{v.modelNumber}</Td>
                      <Td>In Stock</Td>
                      <Td textAlign="right">
                        <Button
                          size="sm"
                          colorScheme={
                            selectedVehicle?.vehicleId === v.vehicleId
                              ? 'blue'
                              : 'gray'
                          }
                          variant={
                            selectedVehicle?.vehicleId === v.vehicleId
                              ? 'solid'
                              : 'outline'
                          }
                          onClick={() => setSelectedVehicle(v)}
                        >
                          {selectedVehicle?.vehicleId === v.vehicleId
                            ? 'Selected'
                            : 'Select'}
                        </Button>
                      </Td>
                    </Tr>
                  ))}
                </Tbody>
              </Table>
            )}
          </ModalBody>

          <ModalFooter>
            <Button variant="ghost" mr={3} onClick={onClose}>
              Close
            </Button>

            {/* 🟢 Nút hủy đơn luôn hiển thị */}
            {order && ![3, 4].includes(order.status) && (
              <Button colorScheme="red" mr={3} onClick={handleCancelClick}>
                Cancel Order
              </Button>
            )}

            {!assignedVehicle && (
              <Button
                colorScheme="green"
                isDisabled={!selectedVehicle}
                onClick={handleAssign}
              >
                Assign
              </Button>
            )}
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
}
