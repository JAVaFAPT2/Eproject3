import React, { useEffect, useState } from 'react';
import {
  VStack,
  HStack,
  Checkbox,
  CheckboxGroup,
  Radio,
  RadioGroup,
  Button,
  Box,
  Text,
  Divider,
  Spinner,
  IconButton,
  Collapse,
  Flex,
} from '@chakra-ui/react';
import { AddIcon, MinusIcon } from '@chakra-ui/icons';
import VehicleModelService from 'services/VehicleModelService';

export default function FilterMenu({ selectedFilters, onChangeFilters }) {
  const [groups, setGroups] = useState([]);
  const [loading, setLoading] = useState(false);

  const seatOptions = ['2', '4'];
  const fuelOptions = ['Gasoline', 'Electric', 'Hybrid'];

  const [openSections, setOpenSections] = useState({
    seats: false,
    fuel: false,
  });

  const toggleSection = (section) =>
    setOpenSections((prev) => ({ ...prev, [section]: !prev[section] }));

  useEffect(() => {
    async function fetchGroups() {
      try {
        setLoading(true);
        const res = await VehicleModelService.get({
          pageNumber: 1,
          pageSize: 100,
        });
        const data = res?.items || [];
        const level1 = data.filter((m) => m.level === 1);
        // ✅ Lưu object: All là option đặc biệt, các model khác là object
        setGroups([{ name: 'All', modelNumber: 'All' }, ...level1]);
      } catch (err) {
        console.error('Error loading model groups:', err);
      } finally {
        setLoading(false);
      }
    }
    fetchGroups();
  }, []);

  const handleGroupChange = (value) =>
    onChangeFilters({ ...selectedFilters, group: value });

  const handleSeatChange = (values) =>
    onChangeFilters({ ...selectedFilters, seat: values });

  const handleFuelChange = (values) =>
    onChangeFilters({ ...selectedFilters, fuelType: values });

  const handleReset = () =>
    onChangeFilters({ group: 'All', seat: [], fuelType: [] });

  return (
    <Box minW="300px">
      {loading ? (
        <HStack justify="center" py={6}>
          <Spinner size="lg" />
        </HStack>
      ) : (
        <VStack align="stretch" spacing={8}>
          {/* Models */}
          <Box>
            <Text fontWeight="500" fontSize="xl" mb={3}>
              Models
            </Text>
            <RadioGroup
              value={selectedFilters.group}
              onChange={handleGroupChange}
            >
              <VStack align="start" spacing={3}>
                {groups.map((m) => (
                  <Radio
                    key={m.modelNumber}
                    value={m.modelNumber} // ✅ gửi modelNumber
                    colorScheme="blackAlpha"
                    fontSize="lg"
                    sx={{ '.chakra-radio__label': { fontSize: 'lg' } }}
                  >
                    {m.name} {/* ✅ hiển thị name */}
                  </Radio>
                ))}
              </VStack>
            </RadioGroup>
          </Box>

          <Divider />

          {/* Seats */}
          <Box>
            <Flex
              justify="space-between"
              align="center"
              cursor="pointer"
              onClick={() => toggleSection('seats')}
            >
              <Text fontWeight="500" fontSize="xl">
                Seats
              </Text>
              <IconButton
                size="sm"
                variant="ghost"
                aria-label="Toggle seats"
                icon={openSections.seats ? <MinusIcon /> : <AddIcon />}
              />
            </Flex>

            <Collapse in={openSections.seats} animateOpacity>
              <CheckboxGroup
                value={selectedFilters.seat}
                onChange={handleSeatChange}
              >
                <VStack align="start" mt={3} spacing={3}>
                  {seatOptions.map((s) => (
                    <Checkbox
                      key={s}
                      value={s}
                      colorScheme="blackAlpha"
                      sx={{ '.chakra-checkbox__label': { fontSize: 'lg' } }}
                    >
                      {s} Seats
                    </Checkbox>
                  ))}
                </VStack>
              </CheckboxGroup>
            </Collapse>
          </Box>

          <Divider />

          {/* Fuel Type */}
          <Box>
            <Flex
              justify="space-between"
              align="center"
              cursor="pointer"
              onClick={() => toggleSection('fuel')}
            >
              <Text fontWeight="500" fontSize="xl">
                Fuel Type
              </Text>
              <IconButton
                size="sm"
                variant="ghost"
                aria-label="Toggle fuel"
                icon={openSections.fuel ? <MinusIcon /> : <AddIcon />}
              />
            </Flex>

            <Collapse in={openSections.fuel} animateOpacity>
              <CheckboxGroup
                value={selectedFilters.fuelType}
                onChange={handleFuelChange}
              >
                <VStack align="start" mt={3} spacing={3}>
                  {fuelOptions.map((f) => (
                    <Checkbox
                      key={f}
                      value={f}
                      colorScheme="blackAlpha"
                      sx={{ '.chakra-checkbox__label': { fontSize: 'lg' } }}
                    >
                      {f}
                    </Checkbox>
                  ))}
                </VStack>
              </CheckboxGroup>
            </Collapse>
          </Box>

          <Divider />

          {/* Reset */}
          <Button
            variant="outline"
            colorScheme="blackAlpha"
            onClick={handleReset}
            w="full"
            borderRadius="md"
          >
            Reset Filters
          </Button>
        </VStack>
      )}
    </Box>
  );
}
