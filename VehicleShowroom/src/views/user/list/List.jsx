import React, { useState, useEffect } from 'react';
import {
  Box,
  Flex,
  Text,
  useDisclosure,
  useBreakpointValue,
} from '@chakra-ui/react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import FilterMenu from 'views/user/list/components/FilterMenu';
import VehicleService from 'services/VehicleService';
import Section from 'views/user/list/components/Section';

export default function List() {
  const { isOpen } = useDisclosure({ defaultIsOpen: true });
  const isMobile = useBreakpointValue({ base: true, md: false });

  const { model: paramModel } = useParams();
  const [searchParams, setSearchParams] = useSearchParams();
  const navigate = useNavigate();

  const [filters, setFilters] = useState({
    group: paramModel || 'All',
    seat: searchParams.get('seat')?.split(',') || [],
    fuelType: searchParams.get('fuelType')?.split(',') || [],
  });

  const [vehicles, setVehicles] = useState([]);

  // ✅ Khi chọn model trong filter → đổi URL path
  useEffect(() => {
    if (
      filters.group &&
      filters.group !== 'All' &&
      filters.group.toLowerCase() !== paramModel?.toLowerCase()
    ) {
      navigate(`/user/models/${filters.group}`, { replace: false });
    }
  }, [filters.group]);

  // ✅ Khi URL param đổi → sync lại state
  useEffect(() => {
    if (paramModel && paramModel !== filters.group) {
      setFilters((prev) => ({ ...prev, group: paramModel }));
    }
  }, [paramModel]);

  // ✅ Cập nhật query param khi filter đổi
  useEffect(() => {
    const params = {};
    if (filters.seat.length > 0) params.seat = filters.seat.join(',');
    if (filters.fuelType.length > 0)
      params.fuelType = filters.fuelType.join(',');

    setSearchParams(params);
  }, [filters.seat, filters.fuelType, setSearchParams]);

  // ✅ Lọc dữ liệu FE
  useEffect(() => {
    async function fetchVehicles() {
      const all = await VehicleService.getAll();
      let filtered = all;

      if (filters.group && filters.group !== 'All') {
        filtered = filtered.filter((v) =>
          v.modelNumber.toLowerCase().includes(filters.group.toLowerCase()),
        );
      }

      if (filters.seat.length > 0) {
        filtered = filtered.filter((v) =>
          v.specs?.some(
            (s) =>
              s.specName.toLowerCase().includes('seat') &&
              filters.seat.includes(s.specValue),
          ),
        );
      }

      if (filters.fuelType.length > 0) {
        filtered = filtered.filter((v) =>
          v.specs?.some(
            (s) =>
              s.specName.toLowerCase().includes('fuel') &&
              filters.fuelType.includes(s.specValue),
          ),
        );
      }

      setVehicles(filtered);
    }

    fetchVehicles();
  }, [filters]);

  return (
    <Box pt="100px" minH="100vh" px={{ base: 4, md: 10 }}>
      <Text fontSize="4xl" fontWeight="600" mb={6}>
        Model Overview
      </Text>

      <Flex gap={10} flexDir={{ base: 'column', md: 'row' }}>
        {/* Sidebar Filter */}
        <Box
          flex={{ base: 'none', md: '0 0 300px' }}
          position={{ md: 'sticky' }}
          top="100px"
          alignSelf="flex-start"
          h="fit-content"
        >
          <FilterMenu
            isOpen={!isMobile || isOpen}
            selectedFilters={filters}
            onChangeFilters={setFilters}
          />
        </Box>

        {/* Vehicle List */}
        <Box flex="1">
          <Section vehicles={vehicles} />
        </Box>
      </Flex>
    </Box>
  );
}
