# Images Folder

This folder contains the puzzle images used by the application.

## Required Images

You need to add the following 24 image files to this folder:

### Animals Pack
- `cat-1.jpg` - Cat image 1
- `cat-2.jpg` - Cat image 2  
- `cat-3.jpg` - Cat image 3
- `cat-4.jpg` - Cat image 4
- `dog-1.jpg` - Dog image 1
- `dog-2.jpg` - Dog image 2
- `dog-3.jpg` - Dog image 3
- `dog-4.jpg` - Dog image 4

### Food & Drinks Pack
- `pizza-1.jpg` - Pizza image 1
- `pizza-2.jpg` - Pizza image 2
- `pizza-3.jpg` - Pizza image 3
- `pizza-4.jpg` - Pizza image 4
- `apple-1.jpg` - Apple image 1
- `apple-2.jpg` - Apple image 2
- `apple-3.jpg` - Apple image 3
- `apple-4.jpg` - Apple image 4

### Sports Pack
- `basketball-1.jpg` - Basketball image 1
- `basketball-2.jpg` - Basketball image 2
- `basketball-3.jpg` - Basketball image 3
- `basketball-4.jpg` - Basketball image 4
- `soccer-1.jpg` - Soccer image 1
- `soccer-2.jpg` - Soccer image 2
- `soccer-3.jpg` - Soccer image 3
- `soccer-4.jpg` - Soccer image 4

## Image Specifications

- Recommended size: 300x300 pixels
- Format: JPG or JPEG
- Each puzzle needs 4 different images of the same subject
- Images should be clear and easily recognizable

## How to Use

1. Add your image files to this folder with the exact names listed above
2. The API will serve these images at `/images/[filename]`
3. The web app will load these images for each puzzle

After adding the images, restart the API server to ensure the static file middleware picks up the new files.
