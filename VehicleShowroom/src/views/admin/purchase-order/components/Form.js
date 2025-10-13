import React, { useEffect, useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalFooter, ModalCloseButton, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react';
import PurchaseOrderService from 'services/PurchaseOrderService';

export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ orderNumber: '', modelNumber: '', quantity: '', status: '' });
  const toast = useToast();

  useEffect(() => {
    if (editing) setForm(editing);
    else setForm({ orderNumber: '', modelNumber: '', quantity: '', status: '' });
  }, [editing]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async () => {
    try {
      if (editing) await PurchaseOrderService.update(editing.id, form);
      else await PurchaseOrderService.create(form);
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
        <ModalHeader>{editing ? 'Edit' : 'Add'} Purchase Order</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}><FormLabel>Order Number</FormLabel><Input name="orderNumber" value={form.orderNumber} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Model</FormLabel><Input name="modelNumber" value={form.modelNumber} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Quantity</FormLabel><Input type="number" name="quantity" value={form.quantity} onChange={handleChange} /></FormControl>
          <FormControl><FormLabel>Status</FormLabel><Input name="status" value={form.status} onChange={handleChange} /></FormControl>
        </ModalBody>
        <ModalFooter><Button colorScheme="blue" onClick={handleSubmit}>{editing ? 'Update' : 'Create'}</Button></ModalFooter>
      </ModalContent>
    </Modal>
  );
}
