import React, { useEffect, useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalFooter, ModalCloseButton, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react';
import ServiceOrderService from 'services/ServiceOrderService';

export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ serviceId: '', vehicleId: '', serviceType: '', status: '' });
  const toast = useToast();
  useEffect(() => { editing ? setForm(editing) : setForm({ serviceId: '', vehicleId: '', serviceType: '', status: '' }); }, [editing]);
  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });
  const handleSubmit = async () => {
    try {
      editing ? await ServiceOrderService.update(editing.id, form) : await ServiceOrderService.create(form);
      toast({ title: 'Saved', status: 'success' }); reload(); onClose();
    } catch { toast({ title: 'Error', status: 'error' }); }
  };
  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>{editing ? 'Edit' : 'Add'} Service Order</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}><FormLabel>Service ID</FormLabel><Input name="serviceId" value={form.serviceId} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Vehicle ID</FormLabel><Input name="vehicleId" value={form.vehicleId} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Type</FormLabel><Input name="serviceType" value={form.serviceType} onChange={handleChange} /></FormControl>
          <FormControl><FormLabel>Status</FormLabel><Input name="status" value={form.status} onChange={handleChange} /></FormControl>
        </ModalBody>
        <ModalFooter><Button colorScheme="blue" onClick={handleSubmit}>{editing ? 'Update' : 'Create'}</Button></ModalFooter>
      </ModalContent>
    </Modal>
  );
}
