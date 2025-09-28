import React, { useEffect, useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalFooter, ModalCloseButton, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react';
import RegistrationService from 'services/RegistrationService';
export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ registrationNumber: '', vehicleId: '', ownerName: '', status: '' });
  const toast = useToast();
  useEffect(() => { editing ? setForm(editing) : setForm({ registrationNumber: '', vehicleId: '', ownerName: '', status: '' }); }, [editing]);
  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });
  const handleSubmit = async () => { try { editing ? await RegistrationService.update(editing.id, form) : await RegistrationService.create(form); toast({ title: 'Saved', status: 'success' }); reload(); onClose(); } catch { toast({ title: 'Error', status: 'error' }); } };
  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>{editing ? 'Edit' : 'Add'} Registration</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}><FormLabel>Reg No</FormLabel><Input name="registrationNumber" value={form.registrationNumber} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Vehicle ID</FormLabel><Input name="vehicleId" value={form.vehicleId} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Owner</FormLabel><Input name="ownerName" value={form.ownerName} onChange={handleChange} /></FormControl>
          <FormControl><FormLabel>Status</FormLabel><Input name="status" value={form.status} onChange={handleChange} /></FormControl>
        </ModalBody>
        <ModalFooter><Button colorScheme="blue" onClick={handleSubmit}>{editing ? 'Update' : 'Create'}</Button></ModalFooter>
      </ModalContent>
    </Modal>
  );
}
