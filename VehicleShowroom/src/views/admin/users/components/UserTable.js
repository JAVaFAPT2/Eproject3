import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { Card, useColorModeValue } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import Pagination from 'components/pagination/Pagination';
import UserService from 'services/UserService';
import RoleService from 'services/RoleService';
import UserHeader from './UserHeader';
import UserList from './UserList';
import UserColumns from './UserColumns';

export default function UserTable() {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const bgColor = useColorModeValue('white', 'navy.800');
  const brandColor = useColorModeValue('brand.500', 'brand.400');

  const [users, setUsers] = useState([]);
  const [roles, setRoles] = useState([]);
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [roleFilter, setRoleFilter] = useState(null);

  const loadUsers = useCallback(
    async (p = 0) => {
      try {
        console.log(roleFilter);
        const res = await UserService.getUsers({
          page: p,
          size: 20,
          keyword: searchInput || undefined,
          roleId: roleFilter || undefined,
        });
        setUsers(res.content || []);
        setTotalPages(res.totalPages || 1);
      } catch (err) {
        console.error(err);
      }
    },
    [searchInput, roleFilter],
  );

  const loadRoles = async () => {
    try {
      const res = await RoleService.getRoles();
      setRoles(res || []);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    loadRoles();
    loadUsers(page);
  }, [loadUsers, page, searchInput, roleFilter]);

  const columns = useMemo(
    () =>
      UserColumns({
        roles,
        roleFilter,
        setRoleFilter,
        bgColor,
        borderColor,
        brandColor,
      }),
    [roles, roleFilter, bgColor, borderColor, brandColor],
  );

  const table = useReactTable({
    data: users,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <Card
      flexDirection="column"
      w="100%"
      borderRadius="16px"
      boxShadow="md"
      bg={bgColor}
    >
      <UserHeader
        searchInput={searchInput}
        setSearchInput={setSearchInput}
        textColor={textColor}
      />
      <UserList table={table} textColor={textColor} />
      {totalPages > 1 && (
        <Pagination
          page={page}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      )}
    </Card>
  );
}
