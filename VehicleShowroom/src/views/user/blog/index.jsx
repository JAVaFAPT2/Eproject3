import React, { useState } from 'react';
import AllFeeds from './components/AllBlogs';
import LatestFeed from './components/LatestBlog';
import RecentFeeds from './components/RecentBlogs';
import FeedSearch from './components/BlogSearch';
import { Box, Grid, GridItem, Heading, Flex } from '@chakra-ui/react';

function BlogPage() {
  const [searchInput, setSearchInput] = useState('');

  return (
    <Box p={6}>
      {/* SEARCH TRÊN CÙNG */}
      <Flex justify="center" mb={8}>
        <FeedSearch searchInput={searchInput} setSearchInput={setSearchInput} />
      </Flex>

      {/* PHẦN TRÊN */}
      <Box mb={12}>
        <Grid templateColumns={{ base: "1fr", md: "2fr 1fr" }} gap={6}>
          {/* Bài viết mới nhất */}
          <GridItem>
            <Heading fontSize="20px" mb={4} fontWeight="500">
              Bài viết mới nhất
            </Heading>
            <LatestFeed />
          </GridItem>

          {/* Bài viết đề xuất */}
          <GridItem>
            <Heading fontSize="20px" mb={4} fontWeight="500">
              Bài viết đề xuất
            </Heading>
            <RecentFeeds />
          </GridItem>
        </Grid>
      </Box>

      {/* PHẦN DƯỚI */}
      <Box>
        <Heading size="md" mb={6}>Tất cả bài viết</Heading>
        {/* ✅ luôn 2 cột trên mobile, 3 cột trên desktop */}
        <AllFeeds columns={{ base: 2, md: 3 }} searchInput={searchInput} />
      </Box>
    </Box>
  );
}

export default BlogPage;
