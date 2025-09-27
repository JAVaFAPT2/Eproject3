import React from 'react';
import { Tr, Td } from '@chakra-ui/react';
import { flexRender } from '@tanstack/react-table';

export default function UserRow({ row, textColor }) {
  return (
    <Tr key={row.id}>
      {row.getVisibleCells().map((cell) => (
        <Td
          key={cell.id}
          borderColor="transparent"
          fontSize="14px"
          color={textColor}
        >
          {flexRender(cell.column.columnDef.cell, cell.getContext())}
        </Td>
      ))}
    </Tr>
  );
}
