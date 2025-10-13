import React, { useEffect, useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalFooter, ModalCloseButton, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react';
import VehicleService from 'services/VehicleService';

export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ vehicleId: '', modelNumber: '', purchasePrice: '', status: '' });
  const toast = useToast();

  useEffect(() => {
    if (editing) setForm(editing);
    else setForm({ vehicleId: '', modelNumber: '', purchasePrice: '', status: '' });
  }, [editing]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async () => {
    try {
      if (editing) await VehicleService.update(editing.id, form);
      else await VehicleService.create(form);
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
        <ModalHeader>{editing ? 'Edit' : 'Add'} Vehicle</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}><FormLabel>ID</FormLabel><Input name="vehicleId" value={form.vehicleId} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Model</FormLabel><Input name="modelNumber" value={form.modelNumber} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Price</FormLabel><Input name="purchasePrice" type="number" value={form.purchasePrice} onChange={handleChange} /></FormControl>
          <FormControl><FormLabel>Status</FormLabel><Input name="status" value={form.status} onChange={handleChange} /></FormControl>
        </ModalBody>
        <ModalFooter><Button colorScheme="blue" onClick={handleSubmit}>{editing ? 'Update' : 'Create'}</Button></ModalFooter>
      </ModalContent>
    </Modal>
  );
}
