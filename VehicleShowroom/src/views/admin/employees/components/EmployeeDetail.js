import React, { useEffect, useState, useCallback } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  Button,
  Text,
  HStack,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  useToast,
  Flex,
  useColorModeValue,
} from '@chakra-ui/react';
import EmployeeService from 'services/EmployeeService';
import { MdArrowDropDown } from 'react-icons/md';

export default function EmployeeDetail({ isOpen, onClose, employee }) {
  const [month, setMonth] = useState(new Date().getMonth() + 1);
  const [year, setYear] = useState(new Date().getFullYear());
  const [records, setRecords] = useState([]);
  const [totalHours, setTotalHours] = useState(0);
  const [salary, setSalary] = useState(0);

  const toast = useToast();

  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');
  const rowBg = useColorModeValue('yellow.100', 'yellow.900');

  const fetchData = useCallback(async () => {
    if (!employee) return;
    try {
      const res = await EmployeeService.getWorkDetail(employee.id, month, year);
      setRecords(res.records || []);
      setTotalHours(res.totalHours || 0);
      setSalary(res.totalSalary || 0);
    } catch (err) {
      console.error(err);
      toast({
        title: 'Error loading data',
        status: 'error',
        duration: 3000,
        isClosable: true,
        position: 'bottom-right',
      });
    }
  }, [employee, month, year, toast]);

  useEffect(() => {
    if (employee) {
      fetchData();
    }
  }, [employee, month, year, fetchData]);

  const usdFormatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  });

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="3xl" isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader bg={headerBg} borderTopRadius="20px">
          Work Details - {employee?.fullName}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Flex justify="space-between" align="center" mb={4} px={2}>
            <HStack spacing={4}>
              <Menu>
                <MenuButton
                  as={Button}
                  rightIcon={<MdArrowDropDown />}
                  variant="outline"
                >
                  Month {month}
                </MenuButton>
                <MenuList>
                  {Array.from({ length: 12 }, (_, i) => (
                    <MenuItem key={i + 1} onClick={() => setMonth(i + 1)}>
                      Month {i + 1}
                    </MenuItem>
                  ))}
                </MenuList>
              </Menu>

              <Menu>
                <MenuButton
                  as={Button}
                  rightIcon={<MdArrowDropDown />}
                  variant="outline"
                >
                  Year {year}
                </MenuButton>
                <MenuList>
                  {Array.from({ length: 5 }, (_, i) => {
                    const y = new Date().getFullYear() - i;
                    return (
                      <MenuItem key={y} onClick={() => setYear(y)}>
                        Year {y}
                      </MenuItem>
                    );
                  })}
                </MenuList>
              </Menu>
            </HStack>

            <HStack spacing={6}>
              <Text fontWeight="bold">
                Total Hours: {totalHours.toFixed(2)} hrs
              </Text>
              <Text fontWeight="bold">
                Total Salary: {usdFormatter.format(salary || 0)}
              </Text>
            </HStack>
          </Flex>

          <Table size="sm" variant="simple">
            <Thead bg={headerBg}>
              <Tr>
                <Th>Check In</Th>
                <Th>Check Out</Th>
                <Th>Hours</Th>
                <Th>Salary</Th>
              </Tr>
            </Thead>
            <Tbody>
              {records.map((r, idx) => (
                <Tr key={idx} bg={idx % 2 === 0 ? 'transparent' : rowBg}>
                  <Td>{new Date(r.checkInTime).toLocaleString()}</Td>
                  <Td>
                    {r.checkOutTime
                      ? new Date(r.checkOutTime).toLocaleString()
                      : '-'}
                  </Td>
                  <Td>{r.workingHours?.toFixed(2) || 0}</Td>
                  <Td>{usdFormatter.format(r.salary || 0)}</Td>
                </Tr>
              ))}
            </Tbody>
          </Table>
        </ModalBody>

        <ModalFooter bg={headerBg} borderBottomRadius="20px">
          <Button onClick={onClose}>Close</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
