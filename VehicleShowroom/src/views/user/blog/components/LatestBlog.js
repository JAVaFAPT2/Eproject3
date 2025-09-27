import React, { useEffect, useState } from "react";
import { Box, Image, Text, Skeleton } from "@chakra-ui/react";
import BlogService from "services/BlogService";

export default function LatestBlog() {
  const [blog, setBlog] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadBlog = async () => {
      try {
        const res = await BlogService.getBlogs("", 0, 1);
        const data = res.content || res;
        setBlog(data[0] || null);
      } catch (err) {
        console.error("Failed to load latest Blog:", err);
      } finally {
        setLoading(false);
      }
    };
    loadBlog();
  }, []);

  if (loading) {
    return (
      <Box>
        <Skeleton height="535px" borderRadius="lg" mb={4} />
        <Skeleton height="20px" mb={2} />
        <Skeleton height="16px" />
      </Box>
    );
  }

  if (!blog) return <Text>Không có dữ liệu</Text>;

  return (
    <Box>
      <Image
        src={blog.thumbnail}
        alt={blog.title}
        borderRadius="lg"
        mb={4}
      />
      <Text fontSize="xl" fontWeight="700" mb={2}>
        {blog.title}
      </Text>
      <Text color="gray.500" noOfLines={3}>
        {blog.summary}
      </Text>
    </Box>
  );
}
