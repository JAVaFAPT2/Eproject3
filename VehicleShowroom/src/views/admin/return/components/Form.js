import React, { useEffect, useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalFooter, ModalCloseButton, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react';
import ReturnService from 'services/ReturnService';
export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ returnId: '', orderId: '', reason: '', status: '' });
  const toast = useToast();
  useEffect(() => { editing ? setForm(editing) : setForm({ returnId: '', orderId: '', reason: '', status: '' }); }, [editing]);
  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });
  const handleSubmit = async () => {
    try {
      editing ? await ReturnService.update(editing.id, form) : await ReturnService.create(form);
      toast({ title: 'Saved', status: 'success' }); reload(); onClose();
    } catch { toast({ title: 'Error', status: 'error' }); }
  };
  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>{editing ? 'Edit' : 'Add'} Return</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}><FormLabel>Return ID</FormLabel><Input name="returnId" value={form.returnId} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Order ID</FormLabel><Input name="orderId" value={form.orderId} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Reason</FormLabel><Input name="reason" value={form.reason} onChange={handleChange} /></FormControl>
          <FormControl><FormLabel>Status</FormLabel><Input name="status" value={form.status} onChange={handleChange} /></FormControl>
        </ModalBody>
        <ModalFooter><Button colorScheme="blue" onClick={handleSubmit}>{editing ? 'Update' : 'Create'}</Button></ModalFooter>
      </ModalContent>
    </Modal>
  );
}
