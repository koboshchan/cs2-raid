"""A2S_INFO query, including the optional challenge. Does not prove plugin load."""
import socket
import sys
query = b"\xff\xff\xff\xffTSource Engine Query\x00"
try:
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as sock:
        sock.settimeout(3)
        sock.connect(("127.0.0.1", 27015))
        sock.send(query)
        data = sock.recv(65535)
        if data.startswith(b"\xff\xff\xff\xffA") and len(data) >= 9:
            sock.send(query + data[5:9])
            data = sock.recv(65535)
        sys.exit(0 if data.startswith(b"\xff\xff\xff\xffI") else 1)
except OSError:
    sys.exit(1)
