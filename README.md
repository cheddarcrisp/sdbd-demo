# Self-Described Binary Document Proof of Concept

This repo contains a proof of concept/demo code that implements the Self-Described Binary Document format. This format is described in a series of blog posts starting at https://crispbyte.dev/blog/2024-01-07-01-SDBD/. This code is most definitely not production ready.

## Issues
1. The `Document` data type violates some of the requirements set for metadata header lists.
2. It has at least one securiy issue in that `content-name` is not checked for being a valid filename.
3. It doesn't do much useful.
4. It implements version 1 of the format which really needs to be worked into a version 2 before wide-spread use.
