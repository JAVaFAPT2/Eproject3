import React, { useEffect, useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalFooter, ModalCloseButton, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react';
import OrderService from 'services/OrderService';

export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ orderNumber: '', totalAmount: '', status: '' });
  const toast = useToast();

  useEffect(() => {
    setForm(editing || { orderNumber: '', totalAmount: '', status: '' });
  }, [editing]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async () => {
    try {
      if (editing) await OrderService.update(editing.id, form);
      else await OrderService.create(form);
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
        <ModalHeader>{editing ? 'Edit' : 'Add'} Order</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}><FormLabel>Order Number</FormLabel><Input name="orderNumber" value={form.orderNumber} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Total</FormLabel><Input name="totalAmount" type="number" value={form.totalAmount} onChange={handleChange} /></FormControl>
          <FormControl><FormLabel>Status</FormLabel><Input name="status" value={form.status} onChange={handleChange} /></FormControl>
        </ModalBody>
        <ModalFooter><Button colorScheme="blue" onClick={handleSubmit}>{editing ? 'Update' : 'Create'}</Button></ModalFooter>
      </ModalContent>
    </Modal>
  );
}
