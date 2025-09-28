import React, { useEffect, useState } from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalFooter, ModalCloseButton, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react';
import GoodsReceiptService from 'services/GoodsReceiptService';

export default function Form({ isOpen, onClose, reload, editing }) {
  const [form, setForm] = useState({ receiptNumber: '', vehicleId: '', receivedDate: '', status: '' });
  const toast = useToast();
  useEffect(() => { editing ? setForm(editing) : setForm({ receiptNumber: '', vehicleId: '', receivedDate: '', status: '' }); }, [editing]);
  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });
  const handleSubmit = async () => {
    try {
      editing ? await GoodsReceiptService.update(editing.id, form) : await GoodsReceiptService.create(form);
      toast({ title: 'Saved', status: 'success' }); reload(); onClose();
    } catch { toast({ title: 'Error', status: 'error' }); }
  };
  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>{editing ? 'Edit' : 'Add'} Receipt</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}><FormLabel>Receipt No</FormLabel><Input name="receiptNumber" value={form.receiptNumber} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Vehicle ID</FormLabel><Input name="vehicleId" value={form.vehicleId} onChange={handleChange} /></FormControl>
          <FormControl mb={3}><FormLabel>Received Date</FormLabel><Input name="receivedDate" value={form.receivedDate} onChange={handleChange} /></FormControl>
          <FormControl><FormLabel>Status</FormLabel><Input name="status" value={form.status} onChange={handleChange} /></FormControl>
        </ModalBody>
        <ModalFooter><Button colorScheme="blue" onClick={handleSubmit}>{editing ? 'Update' : 'Create'}</Button></ModalFooter>
      </ModalContent>
    </Modal>
  );
}
