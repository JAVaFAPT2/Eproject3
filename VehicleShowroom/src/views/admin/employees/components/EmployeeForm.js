import React, { useEffect, useState } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
  ModalCloseButton,
  Button,
  FormControl,
  FormLabel,
  Input,
  useToast,
  useColorModeValue,
} from '@chakra-ui/react';
import EmployeeService from 'services/EmployeeService';

export default function EmployeeForm({
  isOpen,
  onClose,
  reloadEmployees,
  editingEmployee,
}) {
  const [email, setEmail] = useState('');
  const [fullName, setFullName] = useState('');
  const [password, setPassword] = useState('');
  const [hourlyRate, setHourlyRate] = useState('');
  const [loading, setLoading] = useState(false);

  const toast = useToast();

  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  useEffect(() => {
    if (editingEmployee) {
      setEmail(editingEmployee.email || '');
      setFullName(editingEmployee.fullName || '');
      setHourlyRate(editingEmployee.hourlyRate || '');
      setPassword('');
    } else {
      setEmail('');
      setFullName('');
      setHourlyRate('');
      setPassword('');
    }
  }, [editingEmployee]);

  const handleSubmit = async () => {
    setLoading(true);
    try {
      if (editingEmployee) {
        await EmployeeService.updateEmployee(editingEmployee.id, {
          hourlyRate,
        });
        toast({
          title: 'Employee updated successfully',
          status: 'success',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
          containerStyle: { borderRadius: '20px' },
        });
      } else {
        await EmployeeService.createEmployee({
          email,
          fullName,
          password,
          hourlyRate,
        });
        toast({
          title: 'Employee created successfully',
          status: 'success',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
          containerStyle: { borderRadius: '20px' },
        });
      }

      reloadEmployees();
      onClose();
    } catch (error) {
      toast({
        title: 'Error',
        description: error.response?.data?.message || 'Something went wrong',
        status: 'error',
        duration: 3000,
        isClosable: true,
        position: 'bottom-right',
        containerStyle: { borderRadius: '20px' },
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="lg" isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader bg={headerBg} borderTopRadius="20px">
          {editingEmployee ? 'Edit Employee' : 'Create Employee'}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody pb={6}>
          <FormControl mb={3}>
            <FormLabel isRequired>Full Name</FormLabel>
            <Input
              color={textColor}
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
            />
          </FormControl>
          <FormControl mb={3} isRequired>
            <FormLabel>Email</FormLabel>
            <Input
              color={textColor}
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              disabled={!!editingEmployee}
            />
          </FormControl>
          {!editingEmployee && (
            <FormControl mb={3} isRequired>
              <FormLabel>Password</FormLabel>
              <Input
                color={textColor}
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </FormControl>
          )}
          <FormControl isRequired>
            <FormLabel>Hourly Rate</FormLabel>
            <Input
              color={textColor}
              type="number"
              value={hourlyRate}
              onChange={(e) => setHourlyRate(e.target.value)}
            />
          </FormControl>
        </ModalBody>

        <ModalFooter bg={headerBg} borderBottomRadius="20px">
          <Button
            onClick={handleSubmit}
            colorScheme="blue"
            mr={3}
            isLoading={loading}
          >
            {editingEmployee ? 'Update' : 'Create'}
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
