import React, { useEffect, useMemo, useState } from 'react';
import { Box, Card, useToast, useColorModeValue } from '@chakra-ui/react';
import ReportService from 'services/ReportService';
import Table from './components/Table';
import Header from './components/Header';
import Pagination from 'components/pagination/Pagination';
import { getColumns } from './components/Columns';
import mockData from './variables/data.json';

export default function ReportPage() {
  const [data, setData] = useState(mockData);
  const [search, setSearch] = useState('');
  const toast = useToast();
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');

  const load = async () => {
    try {
      const res = await ReportService.getAll();
      setData(res.data || mockData);
    } catch {
      setData(mockData);
      toast({
        title: 'Loaded mock reports',
        status: 'info',
        duration: 3000,
        position: 'bottom-right',
      });
    }
  };

  useEffect(() => { load(); }, []);

  const filtered = useMemo(
    () => data.filter((v) => v.type.toLowerCase().includes(search.toLowerCase())),
    [data, search],
  );

  const columns = useMemo(() => getColumns({ textColor }), [textColor]);

  return (
    <Box pt={{ base: '130px', md: '80px' }}>
      <Card bg={bgColor} borderRadius="16px" boxShadow="md">
        <Header search={search} setSearch={setSearch} />
        <Table data={filtered} columns={columns} />
        <Pagination page={0} totalPages={1} />
      </Card>
    </Box>
  );
}
