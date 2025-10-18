import React, { useState } from "react";
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
} from "@chakra-ui/react";
import { CalendarIcon } from "@chakra-ui/icons";
import MiniCalendar from "components/calendar/MiniCalendar";

export function DatePicker({ label, value, onChange }) {
  const [isOpen, setIsOpen] = useState(false);
  const textColor = useColorModeValue("secondaryGray.900", "white");

  // ✅ Chuẩn hóa hiển thị: nếu có value hợp lệ → format dd/MM/yyyy
  let displayValue = "";
  if (value) {
    const parsed = new Date(value);
    if (!isNaN(parsed)) {
      displayValue = parsed.toLocaleDateString("en-GB"); // dd/MM/yyyy
    }
  }

  // ✅ Khi chọn ngày từ MiniCalendar
  const handleSelectDate = (date) => {
    const formatted = date.toISOString().split("T")[0]; // yyyy-MM-dd
    onChange(formatted);
    setIsOpen(false);
  };

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

        <PopoverContent w="auto" border="none" boxShadow="xl" p={2}>
          <PopoverBody>
            <MiniCalendar
              value={value ? new Date(value) : new Date()}
              onChange={handleSelectDate}
            />
          </PopoverBody>
        </PopoverContent>
      </Popover>
    </FormControl>
  );
}
