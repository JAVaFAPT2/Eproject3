import React, { useState, useEffect } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  FormControl,
  FormLabel,
  Input,
  Button,
  VStack,
  useToast,
  useColorModeValue,
} from '@chakra-ui/react';
import CouponService from 'services/CouponService';

export default function CouponForm({
  isOpen,
  onClose,
  reloadCoupons,
  coupon,
}) {
  const toast = useToast();

  const [code, setCode] = useState('');
  const [discountType, setDiscountType] = useState('');
  const [discountValue, setDiscountValue] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [loading, setLoading] = useState(false);
  const [usageLimit, setUsageLimit] = useState('');
  const [usedCount, setUsedCount] = useState('');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  useEffect(() => {
    if (coupon) {
      setCode(coupon.code || '');
      setDiscountType(coupon.discountType || '');
      setDiscountValue(coupon.discountValue || '');
      setStartDate(coupon.startDate || '');
      setEndDate(coupon.endDate || '');
      setUsageLimit(coupon.usageLimit || '');
      setUsedCount(coupon.usedCount || '');
    } else {
      setCode('');
      setDiscountType('');
      setDiscountValue('');
      setStartDate('');
      setEndDate('');
      setUsageLimit('');
      setUsedCount('');
    }
  }, [coupon, isOpen]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    const payload = {
      code,
      discountType,
      discountValue: discountType === 'percent'
        ? Number(discountValue)        // số %
        : Number(discountValue),       // số tiền
      startDate: startDate ? new Date(startDate).toISOString() : null,
      endDate: endDate ? new Date(endDate).toISOString() : null,
      usageLimit: Number(usageLimit),
      usedCount: coupon ? Number(usedCount) : 0,
      productId: null,
    };

    try {
      if (coupon) {
        await CouponService.updateCoupon(coupon.id, payload);
        toast({
          title: 'Coupon updated successfully',
          status: 'success',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
          containerStyle: { borderRadius: '20px' },
        });
      } else {
        await CouponService.createCoupon(payload);
        toast({
          title: 'Coupon created successfully',
          status: 'success',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
          containerStyle: { borderRadius: '20px' },
        });
      }

      if (reloadCoupons) reloadCoupons();
      onClose();
    } catch (err) {
      console.error("Error response:", err.response?.data || err);
      toast({
        title: 'Error saving coupon',
        status: 'error',
        duration: 3000,
        position: 'bottom-right',
        isClosable: true,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader bg={headerBg} borderTopRadius="20px">
          {coupon ? 'Edit Coupon' : 'Create New Coupon'}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <form id="create-coupon-form" onSubmit={handleSubmit}>
            <VStack spacing={4} align="flex-start">
              <FormControl isRequired>
                <FormLabel>Code</FormLabel>
                <Input
                  color={textColor}
                  value={code}
                  onChange={(e) => setCode(e.target.value)}
                  placeholder="Enter coupon code"
                />
              </FormControl>

              <FormControl isRequired>
                <FormLabel>Discount Value</FormLabel>
                <Input
                  type="number"
                  min={1}          
                  max={10}  
                  color={textColor}
                  value={discountValue}
                  onChange={(e) => setDiscountValue(e.target.value)}
                  placeholder={
                    'Enter percentage (1-100)%'
                  }
                />
              </FormControl>

              <FormControl isRequired>
                <FormLabel>Start Date</FormLabel>
                <Input
                  type="date"
                  color={textColor}
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                />
              </FormControl>
                  
              <FormControl isRequired>
                <FormLabel>End Date</FormLabel>
                <Input
                  type="date"
                  color={textColor}
                  value={endDate}
                  onChange={(e) => setEndDate(e.target.value)}
                />
              </FormControl>

              <FormControl isRequired>
                <FormLabel>Usage Limit</FormLabel>
                <Input
                  type="number"
                  min={1}
                  color={textColor}
                  value={usageLimit}
                  onChange={(e) => setUsageLimit(e.target.value)}
                  placeholder="Enter usage limit"
                />
              </FormControl>
            </VStack>
          </form>
        </ModalBody>

        <ModalFooter bg={headerBg} borderBottomRadius="20px">
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button
            colorScheme="green"
            type="submit"
            form="create-coupon-form"
            isLoading={loading}
          >
            {coupon ? 'Update' : 'Create'}
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
