import { createColumnHelper } from '@tanstack/react-table';
import { Text } from '@chakra-ui/react';
import UserRoleFilter from './UserRoleFilter';

const columnHelper = createColumnHelper();

export default function UserColumns({
  roles,
  roleFilter,
  setRoleFilter,
  bgColor,
  borderColor,
  brandColor,
}) {
  const getRoleName = (roleId) => {
    const role = roles.find((r) => r.id === roleId);
    return role ? role.name : roleId;
  };

  return [
    columnHelper.accessor('email', {
      header: 'EMAIL',
      cell: (info) => (
        <Text fontSize="sm" fontWeight="600">
          {info.getValue()}
        </Text>
      ),
    }),
    columnHelper.accessor('fullName', {
      header: 'FULL NAME',
      cell: (info) => <Text>{info.getValue() || '-'}</Text>,
    }),
    columnHelper.accessor('roleId', {
      header: () => (
        <UserRoleFilter
          roles={roles}
          roleFilter={roleFilter}
          setRoleFilter={setRoleFilter}
          bgColor={bgColor}
          borderColor={borderColor}
          brandColor={brandColor}
        />
      ),
      cell: (info) => <Text>{getRoleName(info.getValue())}</Text>,
    }),
    columnHelper.accessor('createdAt', {
      header: 'CREATED AT',
      cell: (info) => <Text>{new Date(info.getValue()).toLocaleString()}</Text>,
    }),
  ];
}
