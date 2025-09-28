import React, { useEffect, useState } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
  ModalCloseButton,
  FormControl,
  FormLabel,
  Input,
  Button,
} from '@chakra-ui/react';
import EmployeeService from 'services/EmployeeService';
import { useShowToast } from 'utils/helper';

export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ email: '', fullName: '', hourlyRate: '' });
  const { showToast } = useShowToast();

  useEffect(() => {
    if (editing) setForm(editing);
    else setForm({ email: '', fullName: '', hourlyRate: '' });
  }, [editing]);

  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async () => {
    try {
      if (editing) await EmployeeService.update(editing.id, form);
      else await EmployeeService.create(form);
      showToast('Success', 'success');
      reload();
      onClose();
    } catch {
      showToast('Error saving employee', 'error');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>{editing ? 'Edit' : 'Create'} Employee</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}>
            <FormLabel>Email</FormLabel>
            <Input
              name="email"
              value={form.email}
              onChange={handleChange}
              disabled={!!editing}
            />
          </FormControl>
          <FormControl mb={3}>
            <FormLabel>Full Name</FormLabel>
            <Input
              name="fullName"
              value={form.fullName}
              onChange={handleChange}
            />
          </FormControl>
          <FormControl>
            <FormLabel>Hourly Rate</FormLabel>
            <Input
              name="hourlyRate"
              value={form.hourlyRate}
              onChange={handleChange}
            />
          </FormControl>
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="blue" onClick={handleSubmit}>
            {editing ? 'Update' : 'Create'}
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
