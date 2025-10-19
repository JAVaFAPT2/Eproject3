import React, { useState } from 'react';
import {
  FormControl,
  FormLabel,
  InputGroup,
  Input,
  InputRightElement,
  IconButton,
  Popover,
  PopoverTrigger,
  PopoverContent,
  PopoverBody,
  useColorModeValue,
} from '@chakra-ui/react';
import { CalendarIcon } from '@chakra-ui/icons';
import MiniCalendar from 'components/calendar/MiniCalendar';

export function DatePicker({ label, value, onChange }) {
  const [isOpen, setIsOpen] = useState(false);
  const textColor = useColorModeValue('secondaryGray.900', 'white');

  // Parse "yyyy-MM-dd" thành Date LOCAL (tránh UTC)
  const parseLocalDate = (str) => {
    if (!str) return null;
    const [y, m, d] = str.split('-').map(Number);
    return new Date(y, (m || 1) - 1, d || 1);
  };

  // Hiển thị dd/MM/yyyy (từ local date)
  let displayValue = '';
  if (value) {
    const parsed = parseLocalDate(value);
    if (parsed && !isNaN(parsed)) {
      displayValue = parsed.toLocaleDateString('en-GB'); // dd/MM/yyyy
    }
  }

  // Khi chọn ngày → format yyyy-MM-dd (local, không UTC)
  const handleSelectDate = (date) => {
    const y = date.getFullYear();
    const m = String(date.getMonth() + 1).padStart(2, '0');
    const d = String(date.getDate()).padStart(2, '0');
    onChange(`${y}-${m}-${d}`);
    setIsOpen(false);
  };

  const selectedDate = value ? parseLocalDate(value) : new Date();

  return (
    <FormControl isRequired>
      <FormLabel>{label}</FormLabel>
      <Popover
        isOpen={isOpen}
        onClose={() => setIsOpen(false)}
        placement="bottom-start"
        closeOnBlur
      >
        <PopoverTrigger>
          <InputGroup>
            <Input
              readOnly
              value={displayValue}
              placeholder="Select date"
              cursor="pointer"
              color={textColor}
              onClick={() => setIsOpen(true)}
            />
            <InputRightElement>
              <IconButton
                size="sm"
                icon={<CalendarIcon />}
                aria-label="Select date"
                onClick={() => setIsOpen(!isOpen)}
              />
            </InputRightElement>
          </InputGroup>
        </PopoverTrigger>

        {/* key={value} đảm bảo MiniCalendar remount khi value đổi */}
        <PopoverContent
          w="auto"
          border="none"
          boxShadow="xl"
          p={2}
          key={value || 'empty'}
        >
          <PopoverBody>
            <MiniCalendar
              // Dùng local date đã parse, KHÔNG dùng new Date('yyyy-MM-dd') trực tiếp
              value={selectedDate}
              onChange={handleSelectDate}
            />
          </PopoverBody>
        </PopoverContent>
      </Popover>
    </FormControl>
  );
}
