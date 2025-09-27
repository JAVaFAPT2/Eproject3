import React, { useState } from "react";
import {
  Input,
  InputGroup,
  InputLeftElement,
  Icon,
  IconButton,
} from "@chakra-ui/react";
import { SearchIcon } from "@chakra-ui/icons";
import { useNavigate } from "react-router-dom";

export default function BlogSearch() {
  const [value, setValue] = useState("");
  const navigate = useNavigate();

  const handleSearch = (e) => {
    e.preventDefault();
    if (value.trim()) {
      navigate(`/user/blog/search?q=${encodeURIComponent(value.trim())}`);
    }
  };

  return (
    <form onSubmit={handleSearch}>
      <InputGroup w={{ base: "100%", md: "500px" }}>
        <InputLeftElement pointerEvents="none">
          <Icon as={SearchIcon} color="gray.400" />
        </InputLeftElement>

        <Input
          placeholder="Tìm kiếm bài viết..."
          value={value}
          onChange={(e) => setValue(e.target.value)}
          borderRadius="full"   // 🔥 bo tròn input
        />

        <IconButton
          aria-label="Search"
          icon={<SearchIcon />}
          type="submit"
          ml={2}
          borderRadius="full"   // 🔥 bo tròn nút
        />
      </InputGroup>
    </form>
  );
}
