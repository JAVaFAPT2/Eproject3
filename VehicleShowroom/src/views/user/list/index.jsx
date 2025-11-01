import React, { useState, useEffect } from 'react';
import {
  Box,
  Flex,
  Text,
  Spinner,
  useDisclosure,
  useBreakpointValue,
  IconButton,
  Drawer,
  DrawerOverlay,
  DrawerContent,
  DrawerHeader,
  DrawerBody,
  DrawerCloseButton,
} from '@chakra-ui/react';
import { FiFilter } from 'react-icons/fi';
import { useSearchParams } from 'react-router-dom';
import FilterMenu from 'views/user/list/components/FilterMenu';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';
import VehicleSpecService from 'services/VehicleSpecService';
import Section from 'views/user/list/components/Section';

export default function List() {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const isMobile = useBreakpointValue({ base: true, md: false });
  const [searchParams, setSearchParams] = useSearchParams();

  // ✅ đọc từ URL ?parentModelNumber=...
  const [filters, setFilters] = useState({
    group: searchParams.get('parentModelNumber') || 'All',
    seat: searchParams.get('seat')?.split(',') || [],
    fuelType: searchParams.get('fuelType')?.split(',') || [],
  });

  const [models, setModels] = useState([]);
  const [loading, setLoading] = useState(true);

  // ✅ Khi filters thay đổi -> cập nhật URL (hiển thị parentModelNumber)
  useEffect(() => {
    const params = {};
    if (filters.group && filters.group !== 'All')
      params.parentModelNumber = filters.group;
    if (filters.seat.length > 0) params.seat = filters.seat.join(',');
    if (filters.fuelType.length > 0)
      params.fuelType = filters.fuelType.join(',');
    setSearchParams(params);
  }, [filters, setSearchParams]);

  // ✅ Gọi API theo parentModelNumber (hoặc null)
  useEffect(() => {
    const fetchModels = async () => {
      setLoading(true);
      try {
        const params = {
          parentModelNumber:
            filters.group && filters.group !== 'All' ? filters.group : null,
          seats: filters.seat.length > 0 ? filters.seat.join(',') : null,
          fuelType:
            filters.fuelType.length > 0 ? filters.fuelType.join(',') : null,
          pageNumber: 1,
          pageSize: 50,
        };

        const res = await VehicleModelService.get(params);
        const list = (res.items || []).filter((item) => item.level === 2);

        const enriched = await Promise.all(
          list.map(async (m) => {
            let photoUrl = null;
            let specs = [];

            // ✅ Gọi API ảnh
            try {
              const photos = await VehiclePhotoService.getByModelNumber(
                m.modelNumber,
              );
              const items = photos || [];
              const displayPhoto =
                items.find((p) => p.displayOrder === 0)?.photoUrl ||
                items[0]?.photoUrl ||
                items[0]?.url;
              if (displayPhoto) photoUrl = displayPhoto;
            } catch (err) {
              console.warn('Error fetching photos for', m.modelNumber, err);
            }

            // ✅ Gọi API specs
            try {
              const resSpecs = await VehicleSpecService.getByModelNumber(
                m.modelNumber,
              );
              specs = resSpecs || [];
            } catch (err) {
              console.warn('Error fetching specs for', m.modelNumber, err);
            }

            return { ...m, photo: photoUrl, specs };
          }),
        );

        setModels(enriched);
      } catch (err) {
        console.error('Error fetching models:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchModels();
  }, [filters]);

  return (
    <Box pt="100px" minH="100vh" px={{ base: 4, md: 10 }}>
      <Flex justify="space-between" align="center" mb={10}>
        <Text fontSize="4xl" fontWeight="500">
          Model Overview
        </Text>

        {/* 🔹 Icon filter chỉ hiện ở mobile */}
        {isMobile && (
          <IconButton
            icon={<FiFilter />}
            aria-label="Open Filters"
            onClick={onOpen}
            variant="outline"
            borderRadius="md"
          />
        )}
      </Flex>

      <Flex gap={10} flexDir={{ base: 'column', md: 'row' }}>
        {/* 🔹 Sidebar desktop */}
        {!isMobile && (
          <Box flex="0 0 300px" alignSelf="flex-start" h="fit-content">
            <FilterMenu
              selectedFilters={filters}
              onChangeFilters={setFilters}
            />
          </Box>
        )}

        {/* 🔹 Drawer mobile */}
        {isMobile && (
          <Drawer isOpen={isOpen} placement="right" onClose={onClose} size="sm">
            <DrawerOverlay bg="blackAlpha.500" backdropFilter="blur(3px)" />
            <DrawerContent>
              <DrawerCloseButton />
              <DrawerHeader borderBottomWidth="1px">Filters</DrawerHeader>
              <DrawerBody>
                <FilterMenu
                  selectedFilters={filters}
                  onChangeFilters={setFilters}
                />
              </DrawerBody>
            </DrawerContent>
          </Drawer>
        )}

        {/* 🔹 Danh sách models */}
        <Box flex="1">
          {loading ? (
            <Flex justify="center" align="center" h="300px">
              <Spinner size="xl" />
            </Flex>
          ) : (
            <Section vehicles={models} />
          )}
        </Box>
      </Flex>
    </Box>
  );
}
