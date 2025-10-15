import React from 'react';
import {
  IconButton,
  Input,
  InputGroup,
  InputLeftElement,
  useColorModeValue,
} from '@chakra-ui/react';
import { SearchIcon } from '@chakra-ui/icons';

export function SearchBar({
  placeholder,
  borderRadius,
  background,
  value,
  onChange,
  onSearch,
  ...rest
}) {
  const searchIconColor = useColorModeValue('gray.700', 'white');
  const inputBg = useColorModeValue('secondaryGray.300', 'navy.900');
  const inputText = useColorModeValue('gray.700', 'gray.100');

  const handleKeyDown = (e) => {
    if (e.key === 'Enter' && onSearch) onSearch(e.target.value);
  };

  return (
    <InputGroup w={{ base: '100%', md: '200px' }} {...rest}>
      <InputLeftElement>
        <IconButton
          bg="inherit"
          borderRadius="inherit"
          _hover="none"
          _active={{ bg: 'inherit', transform: 'none' }}
          _focus={{ boxShadow: 'none' }}
          icon={<SearchIcon color={searchIconColor} w="15px" h="15px" />}
          onClick={() => onSearch && onSearch(value)}
        />
      </InputLeftElement>
      <Input
        variant="search"
        fontSize="sm"
        bg={background || inputBg}
        color={inputText}
        fontWeight="500"
        placeholder={placeholder || 'Search...'}
        borderRadius={borderRadius || '30px'}
        value={value}
        onChange={onChange}
        onKeyDown={handleKeyDown}
        _placeholder={{ color: 'gray.400', fontSize: '14px' }}
      />
    </InputGroup>
  );
}
