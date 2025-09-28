import React, { useEffect, useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalFooter, ModalCloseButton, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react';
import CustomerService from 'services/CustomerService';

export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ fullName: '', email: '', phone: '' });
  const toast = useToast();

  useEffect(() => {
    if (editing) setForm(editing);
    else setForm({ fullName: '', email: '', phone: '' });
  }, [editing]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async () => {
    try {
      if (editing) await CustomerService.update(editing.id, form);
      else await CustomerService.create(form);
      toast({ title: 'Saved', status: 'success' });
      reload();
      onClose();
    } catch {
      toast({ title: 'Error', status: 'error' });
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>{editing ? 'Edit' : 'Add'} Customer</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}><FormLabel>Full Name</FormLabel><Input name="fullName" value={form.fullName} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Email</FormLabel><Input name="email" value={form.email} onChange={handleChange} /></FormControl>
          <FormControl><FormLabel>Phone</FormLabel><Input name="phone" value={form.phone} onChange={handleChange} /></FormControl>
        </ModalBody>
        <ModalFooter><Button colorScheme="blue" onClick={handleSubmit}>{editing ? 'Update' : 'Create'}</Button></ModalFooter>
      </ModalContent>
    </Modal>
  );
}
