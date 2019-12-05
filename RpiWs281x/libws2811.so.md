To build libws2811.so, follow the normal build instructions https://github.com/jgarff/rpi_ws281x#build and run `scons libws2811.so`

Current libws2811.so version was built from 6b01e53c3f8bbcfd2c49c447d4c6a6e2b0031370 with the following changes:

**Fix ARM64 build target.**

```diff
diff --git a/rpihw.c b/rpihw.c
index 1fa6e44..09d9bfd 100644
--- a/rpihw.c
+++ b/rpihw.c
@@ -403,7 +403,7 @@ const rpi_hw_t *rpi_hw_detect(void)
         return NULL;
     }
     size_t read = fread(&rev, sizeof(uint32_t), 1, f);
-    if (read != sizeof(uint32_t))
+    if (read != 1)
         goto done;
     #if __BYTE_ORDER__ == __ORDER_LITTLE_ENDIAN__
         rev = bswap_32(rev);  // linux,revision appears to be in big endian
```

**Use atomic read for accessing LED colors**

```diff
diff --git a/ws2811.c b/ws2811.c
index 945de96..b4429c8 100644
--- a/ws2811.c
+++ b/ws2811.c
@@ -1174,12 +1174,13 @@ ws2811_return_t  ws2811_render(ws2811_t *ws2811)

         for (i = 0; i < channel->count; i++)                // Led
         {
+            ws2811_led_t packed_color = channel->leds[i];
             uint8_t color[] =
             {
-                channel->gamma[(((channel->leds[i] >> channel->rshift) & 0xff) * scale) >> 8], // red
-                channel->gamma[(((channel->leds[i] >> channel->gshift) & 0xff) * scale) >> 8], // green
-                channel->gamma[(((channel->leds[i] >> channel->bshift) & 0xff) * scale) >> 8], // blue
-                channel->gamma[(((channel->leds[i] >> channel->wshift) & 0xff) * scale) >> 8], // white
+                channel->gamma[(((packed_color >> channel->rshift) & 0xff) * scale) >> 8], // red
+                channel->gamma[(((packed_color >> channel->gshift) & 0xff) * scale) >> 8], // green
+                channel->gamma[(((packed_color >> channel->bshift) & 0xff) * scale) >> 8], // blue
+                channel->gamma[(((packed_color >> channel->wshift) & 0xff) * scale) >> 8], // white
             };

             for (j = 0; j < array_size; j++)               // Color
```
