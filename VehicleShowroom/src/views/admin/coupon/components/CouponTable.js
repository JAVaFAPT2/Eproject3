import React, { useEffect, useMemo, useState } from 'react';
import {
  Box,
  Table,
  Tbody,
  Td,
  Th,
  Tr,
  Thead,
  Card,
  useColorModeValue,
  useToast,
  useDisclosure,
} from '@chakra-ui/react';
import { useReactTable, getCoreRowModel, flexRender } from '@tanstack/react-table';
import Pagination from 'components/pagination/Pagination';
import CouponForm from './CouponForm';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import CouponHeader from './CouponHeader';
import { getCouponColumns } from './CouponColumns';
import CouponService from 'services/CouponService';

export default function CouponTable() {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  const [coupons, setCoupons] = useState([]);
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [editingCoupon, setEditingCoupon] = useState(null);
  const [CouponToDelete, setCouponToDelete] = useState(null);
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);

  const toast = useToast();
  const { isOpen, onOpen, onClose } = useDisclosure();

  useEffect(() => {
    loadCoupons(page);
  }, [page]);

  const loadCoupons = async (p = 0) => {
    try {
      const res = await CouponService.getCoupon(p, 8);
      setCoupons(res.content || []);
      setTotalPages(res.totalPages || 1);
    } catch (err) {
      console.error(err);
    }
  };

  const filteredData = useMemo(() => {
    let data = coupons;
    if (searchInput) {
      data = data.filter((row) =>
        ['code', 'discountType', 'discountValue'].some((key) =>
          String(row[key] || '').toLowerCase().includes(searchInput.toLowerCase()),
        ),
      );
    }
    return data;
  }, [coupons, searchInput]);

  const handleDelete = async () => {
    try {
      await CouponService.deleteCoupon(CouponToDelete.id);
      toast({
        title: 'Coupon deleted successfully',
        status: 'success',
        duration: 3000,
        isClosable: true,
        position: 'bottom-right',
      });
      setCouponToDelete(null);
      loadCoupons(page);
    } catch (err) {
      toast({
        title: 'Failed to delete coupon',
        status: 'error',
        duration: 3000,
        isClosable: true,
        position: 'bottom-right',
      });
    }
  };

  const columns = useMemo(
    () => getCouponColumns(onOpen, setEditingCoupon, setCouponToDelete, setIsConfirmOpen),
    [onOpen],
  );

  const table = useReactTable({
    data: filteredData,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  const handleCouponChanged = () => {
    loadCoupons(page);
    onClose();
  };

  return (
    <>
      <CouponForm
        isOpen={isOpen}
        onClose={() => {
          setEditingCoupon(null);
          onClose();
        }}
        reloadCoupons={handleCouponChanged}
        editingCoupon={editingCoupon}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => setIsConfirmOpen(false)}
        onConfirm={handleDelete}
        title="Delete Coupon"
        message={`Are you sure you want to delete ${CouponToDelete?.fullName}?`}
      />

      <Card flexDirection="column" w="100%" borderRadius="16px" boxShadow="md" bg={bgColor}>
        <CouponHeader searchInput={searchInput} setSearchInput={setSearchInput} onOpen={onOpen} />

        <Box minH={'400px'} overflowX="auto" p={3}>
          <Table variant="simple" bg={bgColor}>
            <Thead bg={headerBg}>
              {table.getHeaderGroups().map((headerGroup) => (
                <Tr key={headerGroup.id}>
                  {headerGroup.headers.map((header) => (
                    <Th key={header.id} borderColor={borderColor} fontSize="12px" color={textColor}>
                      {flexRender(header.column.columnDef.header, header.getContext())}
                    </Th>
                  ))}
                </Tr>
              ))}
            </Thead>
            <Tbody>
              {table.getRowModel().rows.map((row) => (
                <Tr key={row.id}>
                  {row.getVisibleCells().map((cell) => (
                    <Td key={cell.id} borderColor="transparent" fontSize="14px" color={textColor}>
                      {flexRender(cell.column.columnDef.cell, cell.getContext())}
                    </Td>
                  ))}
                </Tr>
              ))}
            </Tbody>
          </Table>
        </Box>

        {totalPages > 1 && (
          <Pagination page={page} totalPages={totalPages} onPageChange={setPage} />
        )}
      </Card>
    </>
  );
}
