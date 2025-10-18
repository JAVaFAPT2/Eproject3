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

export default function AssignForm({ isOpen, onClose, order, onAssigned }) {
  const [vehicles, setVehicles] = useState([]);
  const [selectedVehicle, setSelectedVehicle] = useState(null);
  const [assignedVehicle, setAssignedVehicle] = useState(null);
  const [loading, setLoading] = useState(false);

  const bg = useColorModeValue('white', 'navy.800');

  useEffect(() => {
    if (!isOpen || !order) return;

    const load = async () => {
      setLoading(true);
      try {
        // 🟢 Nếu order đã có vehicle → lấy chi tiết vehicle đó
        if (order.vehicleId) {
          const res = await VehicleService.get({ id: order.vehicleId });
          const vehicle =
            res.vehicle || res.items?.find((v) => v.vehicleId === order.vehicleId);
          setAssignedVehicle(vehicle || null);
          setVehicles([]); // không load danh sách
        } else {
          // 🟢 Chưa assign → lấy danh sách xe In Stock theo model
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

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="4xl" isCentered>
      <ModalOverlay />
      <ModalContent bg={bg} minH="600px">
        <ModalHeader>
          {order?.vehicleId
            ? `Assigned Vehicle for Order #${order?.id}`
            : `Assign Vehicle for Order #${order?.id}`}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          {loading ? (
            <Flex justify="center" py={10}>
              <Spinner />
            </Flex>
          ) : assignedVehicle ? (
            // ✅ Đã assign → hiển thị 1 dòng
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
            // ✅ Danh sách xe có thể assign
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
  );
}
