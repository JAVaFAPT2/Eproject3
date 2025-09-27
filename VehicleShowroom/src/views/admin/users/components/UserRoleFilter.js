import {
  Menu, MenuButton, MenuList, MenuItem, Flex, Text, Box, Button
} from "@chakra-ui/react";
import { MdArrowDropDown } from "react-icons/md";

export default function UserRoleFilter({ roles, roleFilter, setRoleFilter, bgColor, borderColor, brandColor }) {
  return (
    <Menu isLazy>
      <MenuButton
        as={Button}
        size="sm"
        variant="ghost"
        rightIcon={<MdArrowDropDown />}
        _hover={{ bg: "transparent" }}
        _active={{ bg: "transparent" }}
      >
        ROLE
      </MenuButton>
      <MenuList bg={bgColor} borderColor={borderColor}>
        <MenuItem
          bg={bgColor}
          _hover={"transparent"}
          onClick={() => setRoleFilter(null)}
        >
          <Flex w="100%" justify="space-between" align="center">
            <Text fontWeight={!roleFilter ? "bold" : "normal"}>All</Text>
            <Box
              h="36px"
              w="4px"
              bg={!roleFilter ? brandColor : "transparent"}
              borderRadius="5px"
            />
          </Flex>
        </MenuItem>
        {roles.map((role) => (
          <MenuItem
            key={role.id}
            bg={bgColor}
            _hover={"transparent"}
            onClick={() => setRoleFilter(role.id)}
          >
            <Flex w="100%" justify="space-between" align="center">
              <Text fontWeight={roleFilter === role.id ? "bold" : "normal"}>
                {role.name}
              </Text>
              <Box
                h="36px"
                w="4px"
                bg={roleFilter === role.id ? brandColor : "transparent"}
                borderRadius="5px"
              />
            </Flex>
          </MenuItem>
        ))}
      </MenuList>
    </Menu>
  );
}
