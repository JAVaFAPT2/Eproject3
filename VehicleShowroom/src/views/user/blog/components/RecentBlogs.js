import React, { useEffect, useState } from "react";
import { VStack, HStack, Box, Image, Text, Skeleton } from "@chakra-ui/react";
import BlogService from "services/BlogService";

export default function RecentBlogs() {
  const [blogs, setBlogs] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadBlogs = async () => {
      try {
        const res = await BlogService.getBlogs("", 0, 5);
        const data = res.content || res;
        setBlogs(data.slice(1, 5));
      } catch (err) {
        console.error("Failed to load recent Blogs:", err);
      } finally {
        setLoading(false);
      }
    };
    loadBlogs();
  }, []);

  if (loading) {
    return (
      <VStack align="stretch" spacing={5}>
        {[...Array(4)].map((_, idx) => (
          <HStack key={idx} spacing={4}>
            <Skeleton boxSize="119px" borderRadius="md" />
            <Box flex="1">
              <Skeleton height="20px" mb={2} />
              <Skeleton height="16px" />
            </Box>
          </HStack>
        ))}
      </VStack>
    );
  }

  if (blogs.length === 0) return <Text>Không có dữ liệu</Text>;

  return (
    <VStack align="stretch" spacing={5}>
      {blogs.map((blog, idx) => (
        <HStack key={idx} align="start" spacing={4}>
          <Image
            src={blog.thumbnail}
            alt={blog.title}
            w="155px"            
            h="119px"           
            objectFit="cover"
            borderRadius="md"
          />
          <Box>
            <Text fontSize="md" fontWeight="600" noOfLines={2}>
              {blog.title}
            </Text>
            <Text fontSize="sm" color="gray.500">
              {blog.date}
            </Text>
          </Box>
        </HStack>
      ))}
    </VStack>
  );
}
