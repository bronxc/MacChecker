# MacChecker
Basic tool for looking up a MAC addresses OUI.

## Usage

Provide a file that includes one MAC per line. The script will reach out to the wireshark OUI database to pull in the most recent OUI to mappings to vendors.

The information being pulled comes from the following location:
https://code.wireshark.org/review/gitweb?p=wireshark.git;a=blob_plain;f=manuf

```
> MacChecker.exe [MACs.txt]
```
