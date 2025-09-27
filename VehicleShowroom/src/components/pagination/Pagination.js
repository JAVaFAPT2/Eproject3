import React, { useState } from 'react';
import {
  HStack,
  Button,
  Input,
  useToast,
  useColorModeValue,
} from '@chakra-ui/react';

function Pagination({ page, totalPages, onPageChange }) {
  const [showInput, setShowInput] = useState(false);
  const [inputValue, setInputValue] = useState(page + 1);
  const toast = useToast();

  const activeColor = useColorModeValue('brand.500', 'navy.700');

  const pageNumbers = [];
  const delta = 2;

  let start = Math.max(1, page + 1 - delta);
  let end = Math.min(totalPages, page + 1 + delta);

  for (let i = start; i <= end; i++) pageNumbers.push(i);

  const handleGoToPage = () => {
    const n = Number(inputValue);
    if (n >= 1 && n <= totalPages) {
      onPageChange(n - 1);
      setShowInput(false);
    } else {
      toast({
        title: 'Invalid page number',
        status: 'error',
        duration: 2000,
        isClosable: true,
      });
    }
  };

  return (
    <HStack spacing={2} justify="center" mt={4}>
      {/* Prev */}
      {page > 0 && (
        <Button size="sm" onClick={() => onPageChange(page - 1)}>
          &lt;
        </Button>
      )}

      {/* First page */}
      {start > 1 && (
        <>
          <Button size="sm" onClick={() => onPageChange(0)}>
            1
          </Button>
          {start > 2 && (
            <Button size="sm" onClick={() => setShowInput(true)}>
              ...
            </Button>
          )}
        </>
      )}

      {/* Page numbers around current page */}
      {pageNumbers.map((p) => (
        <Button
          key={p}
          size="sm"
          bg={p === page + 1 ? activeColor : 'transparent'}
          onClick={() => onPageChange(p - 1)}
          color={p === page + 1 ? 'white' : 'grey.500'}
        >
          {p}
        </Button>
      ))}

      {/* Last page */}
      {end < totalPages && (
        <>
          {end < totalPages - 1 && (
            <Button size="sm" onClick={() => setShowInput(true)}>
              ...
            </Button>
          )}
          <Button size="sm" onClick={() => onPageChange(totalPages - 1)}>
            {totalPages}
          </Button>
        </>
      )}

      {/* Next */}
      {page < totalPages - 1 && (
        <Button size="sm" onClick={() => onPageChange(page + 1)}>
          &gt;
        </Button>
      )}

      {/* Input for ... */}
      {showInput && (
        <Input
          size="sm"
          w="50px"
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') handleGoToPage();
          }}
          onBlur={() => setShowInput(false)}
        />
      )}
    </HStack>
  );
}

export default Pagination;
