import React, { useEffect, useState, useMemo } from 'react';
import {
  Box,
  Heading,
  SimpleGrid,
  Text,
  Image,
  Flex,
  Spinner,
} from '@chakra-ui/react';
import { useLocation } from 'react-router-dom';
import BlogService from 'services/BlogService';
import BlogSearch from '../components/BlogSearch';

export default function BlogSearchPage() {
  const [searchInput, setSearchInput] = useState('');

  const location = useLocation();
  const query = new URLSearchParams(location.search).get('q') || '';

  const [blogs, setBlogs] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadBlogs = async () => {
      try {
        const res = await BlogService.getBlogs('', 0, 50);
        setBlogs(res.content || res);
      } catch (err) {
        console.error('Failed to load blogs:', err);
      } finally {
        setLoading(false);
      }
    };
    loadBlogs();
  }, []);

  const filteredBlogs = useMemo(() => {
    if (!query) return blogs;
    return blogs.filter((blog) =>
      (blog.title || '').toLowerCase().includes(query.toLowerCase()),
    );
  }, [blogs, query]);

  return (
    <Box p={6}>
      <Flex justify="center" mb={8}>
        <BlogSearch searchInput={searchInput} setSearchInput={setSearchInput} />
      </Flex>
      <Heading size="md" fontWeight="400" mb={6}>
        Kết quả tìm kiếm cho: "{query}"
      </Heading>

      {loading ? (
        <Flex justify="center" align="center" minH="300px">
          <Spinner size="lg" color="brand.500" thickness="4px" speed="0.65s" />
        </Flex>
      ) : filteredBlogs.length === 0 ? (
        <Text textAlign="center">Không tìm thấy kết quả nào.</Text>
      ) : (
        <SimpleGrid columns={{ base: 1, md: 2, lg: 3 }} spacing={6}>
          {filteredBlogs.map((blog, idx) => (
            <Box
              key={idx}
              borderWidth="1px"
              borderRadius="lg"
              overflow="hidden"
              p={4}
            >
              <Image
                src={blog.thumbnail}
                alt={blog.title}
                borderRadius="md"
                mb={3}
              />
              <Text fontWeight="bold" noOfLines={2}>
                {blog.title}
              </Text>
              <Text fontSize="sm" color="gray.500" noOfLines={2}>
                {blog.summary}
              </Text>
            </Box>
          ))}
        </SimpleGrid>
      )}
    </Box>
  );
}
